using Bliblioteca.Core.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bliblioteca.Infraestructura.Persistencia.EntityConfiguration
{
    public class LibroConfiguration : IEntityTypeConfiguration<Libros>
    {
        public void Configure(EntityTypeBuilder<Libros> builder)
        {
            builder.ToTable("Libros");
            builder.HasKey(l => l.Id);
            builder.Property(l => l.Descripcion).HasMaxLength(750);
            builder.Property(l => l.Titulo).IsRequired().HasMaxLength(200); 
        }
    }
}
