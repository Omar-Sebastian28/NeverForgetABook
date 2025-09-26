using Bliblioteca.Core.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Bliblioteca.Infraestructura.Persistencia.Contexts
{
    public class BibliotecaContext : DbContext
    {
        public BibliotecaContext(DbContextOptions<BibliotecaContext> options) : base(options) { }

        public DbSet<Libros> Libro { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }   
    }
}
