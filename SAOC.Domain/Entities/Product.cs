using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAOC.Domain.Entities
{
    public class Product
    {
        /** IdProducto,Nombre,Categoría
            1,Producto_1,Juguetes **/

        public string IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Categoría { get; set; }
    }
}
