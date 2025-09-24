using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAOC.Domain.Entities
{
    public class Source
    {
        /** IdFuente,TipoFuente,FechaCarga
            F001,Web,2025-04-10 **/
        public string IdFuente { get; set; }
        public string TipoFuente { get; set; }
        public DateTime FechaCarga { get; set; } 
        
    }
}
