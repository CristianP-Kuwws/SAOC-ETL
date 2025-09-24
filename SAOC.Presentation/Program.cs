using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SAOC.Application.Dtos.Product;
using SAOC.Application.Dtos.Source;
using SAOC.Application.ETL;
using SAOC.Application.Interfaces.ETL;
using SAOC.Application.Interfaces.Repositories;
using SAOC.Application.Interfaces.Services;
using SAOC.Application.Services.Product;
using SAOC.Application.Services.SourceServ;
using SAOC.IOC.Dependencies;
using System;
using System.IO;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // Load configuration
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var sourceCsvPath = configuration["Pipeline:CsvPaths:Sources"];
        var productCsvPath = configuration["Pipeline:CsvPaths:Products"];

        // Setup 
        var services = new ServiceCollection();
        services.AddSourceDependency(connectionString); 
        services.AddProductDependency(connectionString); 
        services.AddScoped(typeof(IGenericETLService<>), typeof(GenericETLService<>));
        var serviceProvider = services.BuildServiceProvider();

        try
        {
            // Source ETL
            if (!string.IsNullOrEmpty(sourceCsvPath) && File.Exists(sourceCsvPath))
            {
                Console.WriteLine("=== Source ETL Preview ===");
                var sourcePreviewService = serviceProvider.GetRequiredService<ISourceETLService>();
                await sourcePreviewService.ExecuteAsync(sourceCsvPath);
                Console.WriteLine("Source ETL completed successfully.\n");
            }
            else
            {
                Console.WriteLine("Source CSV not found or path is empty.");
            }

            // Product ETL
            if (!string.IsNullOrEmpty(productCsvPath) && File.Exists(productCsvPath))
            {
                Console.WriteLine("=== Product ETL Preview ===");
                var productPreviewService = serviceProvider.GetRequiredService<IProductETLService>();
                await productPreviewService.ExecuteAsync(productCsvPath);
                Console.WriteLine("Product ETL completed successfully.\n");
            }
            else
            {
                Console.WriteLine("Product CSV not found or path is empty.");
            }

            Console.WriteLine("All ETL processes finished.");
        }
        catch (FileNotFoundException fex)
        {
            Console.WriteLine($"File not found: {fex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
