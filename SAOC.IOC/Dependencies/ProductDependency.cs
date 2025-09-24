using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SAOC.Application.Dtos.Source.Validators;
using SAOC.Application.Dtos.Source;
using SAOC.Application.Interfaces.Repositories;
using SAOC.Application.Interfaces.Services;
using SAOC.Persistence.Helpers;
using SAOC.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAOC.Application.Dtos.Product;
using SAOC.Application.Dtos.Product.Validators;
using SAOC.Application.Services.Product;
using SAOC.Persistence.Repositories.Product;
using SAOC.Application.ETL;
using SAOC.Application.Interfaces.ETL;

namespace SAOC.IOC.Dependencies
{
    public static class ProductDependency
    {
        public static IServiceCollection AddProductDependency(this IServiceCollection services, string connectionString)
        {
            // Repositories
            services.AddScoped<IProductRepository>(sp =>
                new ProductRepository(new StoredProcedureExecutor(connectionString)));
            services.AddScoped<IBaseRepository<ProductDto>>(sp =>
                sp.GetRequiredService<IProductRepository>()); // clave para GenericETL

            // Validator
            services.AddScoped<IValidator<ProductDto>, ProductValidator>();

            // Generic ETL Service
            services.AddScoped<IGenericETLService<ProductDto>, GenericETLService<ProductDto>>();

            // Services (ETL)
            services.AddScoped<IProductETLService, ProductETLService>();

            return services;
        }
    }
}
