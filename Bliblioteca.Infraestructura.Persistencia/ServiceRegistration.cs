using Bliblioteca.Core.Domain.Interfaces;
using Bliblioteca.Infraestructura.Persistencia.Contexts;
using Bliblioteca.Infraestructura.Persistencia.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bliblioteca.Infraestructura.Persistencia
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            if (configuration.GetValue<bool>("InMemmory"))
            {
                services.AddDbContext<BibliotecaContext>(opt => opt.UseInMemoryDatabase("InMemoryBliblioteca"));
            }
            else
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                services.AddDbContext<BibliotecaContext>(opt => opt.UseSqlServer(connectionString, m => m.MigrationsAssembly(typeof(BibliotecaContext).Assembly.FullName)), ServiceLifetime.Scoped);
            }

            #region Inyeccion de dependencias
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ILibroRepository, LibroRepository>();
            #endregion
        }
    }
}
