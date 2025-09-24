using CsvHelper;
using FluentValidation;
using SAOC.Application.Dtos.Product;
using SAOC.Application.Dtos.Source;
using SAOC.Application.ETL;
using SAOC.Application.Interfaces.ETL;
using SAOC.Application.Interfaces.Repositories;
using SAOC.Application.Interfaces.Services;
using SAOC.Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAOC.Application.Services.SourceServ
{
    public class SourceETLService : ISourceETLService
    {
        private readonly IGenericETLService<SourceDto> _etl;

        public SourceETLService(IGenericETLService<SourceDto> etl)
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
                throw new ApplicationException($"Error in SourceETLService.ExecuteAsync: {ex.Message}", ex);
            }
        }
    }
}

