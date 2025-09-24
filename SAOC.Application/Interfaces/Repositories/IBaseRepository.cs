using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAOC.Application.Interfaces.Repositories
{
    public interface IBaseRepository<TDto>
    {
        Task AddRangeAsync(IEnumerable<TDto> items);
    }

}
