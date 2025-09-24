using Microsoft.Extensions.DependencyInjection;
using SAOC.Application.Interfaces.Repositories;
using SAOC.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SAOC.Persistence.Context;
using FluentValidation;
using SAOC.Application.Dtos.Source;
using SAOC.Application.Dtos.Source.Validators;
using SAOC.Application.Services.SourceServ;
using SAOC.Persistence.Repositories.SourceRep;
using SAOC.Application.Dtos.Product;
using SAOC.Application.ETL;
using SAOC.Application.Interfaces.ETL;

namespace SAOC.IOC.Dependencies
{
    public static class SourceDependency
    {
        public static IServiceCollection AddSourceDependency(this IServiceCollection services, string connectionString)
        {
            // DbContext
            services.AddDbContext<SAOCContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            // Repositories
            services.AddScoped<ISourceRepository, SourceRepository>();
            services.AddScoped<IBaseRepository<SourceDto>>(sp =>
                sp.GetRequiredService<ISourceRepository>()); 

            // Validator
            services.AddScoped<IValidator<SourceDto>, SourceValidator>();

            // Generic ETL Service
            services.AddScoped<IGenericETLService<SourceDto>, GenericETLService<SourceDto>>();

            // Services (ETL)
            services.AddScoped<ISourceETLService, SourceETLService>();

            return services;
        }
    }
}