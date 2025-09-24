using CsvHelper;
using FluentValidation;
using SAOC.Application.Dtos.Product;
using SAOC.Application.Dtos.Product.Validators;
using SAOC.Application.ETL;
using SAOC.Application.Interfaces.ETL;
using SAOC.Application.Interfaces.Repositories;
using SAOC.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAOC.Application.Services.Product
{
    public class ProductETLService : IProductETLService
    {
        private readonly IGenericETLService<ProductDto> _etl;

        public ProductETLService(IGenericETLService<ProductDto> etl)
        {
            _etl = etl;
        }

        public async Task ExecuteAsync(string csvPath)
        {
            try
            {
                await _etl.ExecuteAsync(csvPath);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error in ProductETLService.ExecuteAsync: {ex.Message}", ex);
            }
        }
    }
}
