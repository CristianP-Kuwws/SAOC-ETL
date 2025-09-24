using Microsoft.EntityFrameworkCore;
using SAOC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SAOC.Persistence.Context
{
    public class SAOCContext : DbContext
    {
        public SAOCContext(DbContextOptions<SAOCContext> options) : base(options)
        {
        }
        public DbSet<Source> Sources { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // asegura Liskov

            // Aplica configuraciones automaticas  
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
