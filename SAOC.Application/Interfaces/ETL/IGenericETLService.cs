using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAOC.Application.Interfaces.ETL
{
    public interface IGenericETLService<TDto>
    {
        Task ExecuteAsync(string csvPath);
    }

}
