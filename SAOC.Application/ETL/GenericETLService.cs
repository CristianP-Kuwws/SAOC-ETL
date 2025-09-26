using CsvHelper;
using FluentValidation;
using SAOC.Application.Interfaces.ETL;
using SAOC.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SAOC.Application.ETL
{
    public class GenericETLService<TDto> : IGenericETLService<TDto>
    {
        private readonly IBaseRepository<TDto> _repository;
        private readonly IValidator<TDto> _validator;

        // *** MÉTODO SIMPLE PARA CONVERTIR CÓDIGOS ***
        private readonly string[] _codeProperties = { "IdFuente", "IdComment", "IdReview", "IdCliente", "IdProducto" };

        public GenericETLService(IBaseRepository<TDto> repository, IValidator<TDto> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task ExecuteAsync(string csvPath)
        {
            try
            {
                if (!File.Exists(csvPath))
                    throw new FileNotFoundException("CSV file not found.", csvPath);

                List<TDto> items;

                // Read CSV
                using (var reader = new StreamReader(csvPath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Context.Configuration.HeaderValidated = null;
                    csv.Context.Configuration.MissingFieldFound = null;
                    items = csv.GetRecords<TDto>().ToList();
                }

                int nullCount = 0;
                int invalidCount = 0;
                int duplicateCount = 0;

                // Remove nulls
                var nonNullItems = items.Where(x => x != null).ToList();
                nullCount = items.Count - nonNullItems.Count;

                // Normalize, convert codes and validate
                var validItems = new List<TDto>();
                foreach (var item in nonNullItems)
                {
                    // Normalize strings
                    foreach (var prop in typeof(TDto).GetProperties()
                               .Where(p => p.PropertyType == typeof(string) && p.CanRead && p.CanWrite))
                    {
                        var val = (string?)prop.GetValue(item);
                        if (!string.IsNullOrWhiteSpace(val))
                            prop.SetValue(item, val.Trim().ToUpperInvariant());
                    }

                    // Convert code properties to int
                    ConvertCodesToInts(item);

                    // Validate
                    var result = _validator.Validate(item);
                    if (result.IsValid)
                        validItems.Add(item);
                    else
                        invalidCount++;
                }

                // Detect duplicates by Id*
                var idProp = typeof(TDto).GetProperties()
                    .FirstOrDefault(p => p.Name.StartsWith("Id", StringComparison.OrdinalIgnoreCase));

                List<TDto> distinctItems = validItems;
                if (idProp != null)
                {
                    distinctItems = validItems
                        .GroupBy(x => idProp.GetValue(x))
                        .Select(g => g.First())
                        .ToList();
                    duplicateCount = validItems.Count - distinctItems.Count;
                }

                // Insert into database
                await _repository.AddRangeAsync(distinctItems);

                // Results
                var typeName = typeof(TDto).Name;
                Console.WriteLine($"ETL completed for {typeName}:");
                Console.WriteLine($"- Inserted: {distinctItems.Count}");
                Console.WriteLine($"- Removed nulls: {nullCount}");
                Console.WriteLine($"- Removed invalid: {invalidCount}");
                Console.WriteLine($"- Removed duplicates: {duplicateCount}");
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error executing ETL for {typeof(TDto).Name}.", ex);
            }
        }

        // *** MÉTODO SIMPLE PARA CONVERTIR CÓDIGOS ***
        private void ConvertCodesToInts(TDto item)
        {
            foreach (var propName in _codeProperties)
            {
                var prop = typeof(TDto).GetProperty(propName);
                if (prop?.CanRead == true && prop.CanWrite)
                {
                    var value = prop.GetValue(item)?.ToString();
                    if (!string.IsNullOrEmpty(value))
                    {
                        // Extraer números: F001 -> 1, C123 -> 123
                        var numbers = new string(value.Where(char.IsDigit).ToArray());
                        if (int.TryParse(numbers, out var intValue))
                        {
                            if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?))
                                prop.SetValue(item, intValue);
                            else if (prop.PropertyType == typeof(string))
                                prop.SetValue(item, intValue.ToString());
                        }
                    }
                }
            }
        }
    }
}
