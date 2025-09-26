using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SAOC.Application.Dtos.Product;
using SAOC.Application.Dtos.Source;
using SAOC.Application.ETL;
using SAOC.Application.Interfaces.ETL;
using SAOC.Application.Interfaces.Repositories;
using SAOC.Application.Interfaces.Services;
using SAOC.Application.Interfaces.Helpers;
using SAOC.Application.Services.Product;
using SAOC.Application.Services.SourceServ;
using SAOC.Application.Services.Helpers;
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

        // Setup services
        var services = new ServiceCollection();
        services.AddSourceDependency(connectionString);
        services.AddProductDependency(connectionString);
        services.AddScoped(typeof(IGenericETLService<>), typeof(GenericETLService<>));
        services.AddScoped<ICsvReaderService, CsvReaderService>();
        var serviceProvider = services.BuildServiceProvider();

        var csvReader = serviceProvider.GetRequiredService<ICsvReaderService>();

        try
        {
            // Source ETL
            if (!string.IsNullOrEmpty(sourceCsvPath))
            {
                Console.WriteLine("=== Source ETL Preview ===");

                var sources = await csvReader.ReadAsync<SourceDto>(sourceCsvPath);
                Console.WriteLine($"Read {sources.Count} records from source CSV.\n");

                // show on console
                foreach (var s in sources)
                {
                    Console.WriteLine(
                        $"IdFuente: {s.IdFuente}, TipoFuente: {s.TipoFuente}, FechaCarga: {s.FechaCarga}"
                    );
                }

                // ETL
                var sourcePreviewService = serviceProvider.GetRequiredService<ISourceETLService>();
                await sourcePreviewService.ExecuteAsync(sourceCsvPath);

                Console.WriteLine("\nSource ETL completed successfully.\n");
            }
            else
            {
                Console.WriteLine("Source CSV path is empty.");
            }

            // Product ETL
            if (!string.IsNullOrEmpty(productCsvPath))
            {
                Console.WriteLine("=== Product ETL Preview ===");

                var products = await csvReader.ReadAsync<ProductDto>(productCsvPath);
                Console.WriteLine($"Read {products.Count} records from product CSV.\n");

                // Show on console
                foreach (var p in products)
                {
                    Console.WriteLine(
                        $"IdProducto: {p.IdProducto}, Nombre: {p.Nombre}, Categoria: {p.Categoría}"
                    );
                }
                // ETL
                var productPreviewService = serviceProvider.GetRequiredService<IProductETLService>();
                await productPreviewService.ExecuteAsync(productCsvPath);

                Console.WriteLine("\nProduct ETL completed successfully.\n");
            }
            else
            {
                Console.WriteLine("Product CSV path is empty.");
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

