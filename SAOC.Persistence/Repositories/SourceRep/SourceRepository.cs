using SAOC.Application.Dtos.Source;
using SAOC.Application.Interfaces.Repositories;
using SAOC.Domain.Entities;
using SAOC.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAOC.Persistence.Repositories.SourceRep
{
    public class SourceRepository : ISourceRepository
    {
        private readonly SAOCContext _context;

        public SourceRepository(SAOCContext context)
        {
            _context = context;
        }

        public async Task AddRangeAsync(IEnumerable<SourceDto> sources)
        {
            try
            {
                var entities = new List<Source>();
                foreach (var s in sources)
                {
                    entities.Add(new Source
                    {
                        IdFuente = s.IdFuente,
                        TipoFuente = s.TipoFuente,
                        FechaCarga = s.FechaCarga
                    });
                }

                _context.Sources.AddRange(entities);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while inserting sources via EF Core.", ex);
            }
        }
    }

}

