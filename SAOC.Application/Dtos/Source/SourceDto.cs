using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAOC.Application.Dtos.Source
{
    public record SourceDto
    {
        public string IdFuente { get; set; }
        public string TipoFuente { get; set; }
        public DateTime FechaCarga { get; set; }

    }
}
