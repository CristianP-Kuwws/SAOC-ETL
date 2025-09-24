using CsvHelper;
using SAOC.Application.Interfaces.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAOC.Application.Services.Helpers
{
    public class CsvReaderService : ICsvReaderService
    {
        public async Task<List<TDto>> ReadAsync<TDto>(string csvPath)
        {
            if (!File.Exists(csvPath))
                throw new FileNotFoundException("CSV file not found.", csvPath);

            var result = new List<TDto>();

            using var reader = new StreamReader(csvPath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            await foreach (var record in csv.GetRecordsAsync<TDto>())
                result.Add(record);

            return result;
        }
    }
}
