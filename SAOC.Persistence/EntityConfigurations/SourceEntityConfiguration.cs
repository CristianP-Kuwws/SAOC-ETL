using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SAOC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAOC.Persistence.EntityConfigurations
{
    public class SourceEntityConfiguration : IEntityTypeConfiguration<Source>
    {
        public void Configure(EntityTypeBuilder<Source> builder)
        {
            // Tabla
            builder.ToTable("Fuentes", t =>
            {
                t.HasCheckConstraint("chk_TipoFuente", "TipoFuente IN ('Web', 'CSV', 'Red Social')");
            });

            // Llave primaria
            builder.HasKey(s => s.IdFuente);

            // Propiedades
            builder.Property(s => s.IdFuente)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(s => s.TipoFuente)
                .IsRequired()
                .HasMaxLength(15);

            builder.Property(s => s.FechaCarga)
                .IsRequired()
                .HasColumnType("date");
        }
    }
}
