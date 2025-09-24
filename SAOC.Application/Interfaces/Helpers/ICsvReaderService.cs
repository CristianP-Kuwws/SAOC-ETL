using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAOC.Application.Interfaces.Helpers
{
    public interface ICsvReaderService
    {
        Task<List<TDto>> ReadAsync<TDto>(string csvPath);
    }
}
