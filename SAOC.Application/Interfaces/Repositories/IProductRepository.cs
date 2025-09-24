using SAOC.Application.Dtos.Product;
using SAOC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAOC.Application.Interfaces.Repositories
{
    public interface IProductRepository : IBaseRepository<ProductDto> { }

    // Task<IEnumerable<ProductDto>> GetAllAsync(); 
    // Fue una posible idea, de momento no implementada porque segun el requerimiento, esto no es estrictamente necesario.

}

