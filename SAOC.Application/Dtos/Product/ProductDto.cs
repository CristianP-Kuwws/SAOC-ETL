using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAOC.Application.Dtos.Product
{
    public record ProductDto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Categoría { get; set; }

    }
}
