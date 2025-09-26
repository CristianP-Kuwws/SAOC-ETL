using SAOC.Application.Dtos.Product;
using SAOC.Application.Interfaces.Helpers;
using SAOC.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAOC.Persistence.Repositories.Product
{
    public class ProductRepository : IProductRepository
    {
        private readonly IStoredProcedureExecutor _spExecutor;

        public ProductRepository(IStoredProcedureExecutor spExecutor)
        {
            _spExecutor = spExecutor;
        }

        public async Task AddRangeAsync(IEnumerable<ProductDto> products)
        {
            try
            {
                var tasks = products.Select(async product =>
                {
                    var parameters = new Dictionary<string, object>
                    {
                        { "p_IdProducto", product.IdProducto },
                        { "p_Nombre", product.Nombre },
                        { "p_Categoria", product.Categoría }
                    };

                    var result = await _spExecutor.ExecuteNonQueryAsync("InsertProduct", parameters);

                    if (result.StartsWith("Error", StringComparison.OrdinalIgnoreCase) ||
                        result.StartsWith("Exception", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new Exception(result);
                    }
                });

                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while inserting products. Inner: {ex.Message}", ex);
            }
        }
    }
}
