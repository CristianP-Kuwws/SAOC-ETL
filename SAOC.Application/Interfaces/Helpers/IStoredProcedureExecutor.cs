using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAOC.Application.Interfaces.Helpers
{
    public interface IStoredProcedureExecutor
    {
        Task<string> ExecuteNonQueryAsync(string procedureName, Dictionary<string, object> parameters);
    }
}
