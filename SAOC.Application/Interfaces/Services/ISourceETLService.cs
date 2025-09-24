using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAOC.Application.Interfaces.Services
{
    public interface ISourceETLService
    {
        Task ExecuteAsync(string csvPath);
    }
}
