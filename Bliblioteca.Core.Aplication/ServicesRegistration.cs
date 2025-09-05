using Bliblioteca.Core.Aplication.Interfaces;
using Bliblioteca.Core.Aplication.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Bliblioteca.Core.Aplication
{ 
    public static class ServicesRegistration
    {
        public static void AddAplicationServices(this IServiceCollection services)
        {
            #region AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            #endregion


            #region Configurando inyeccion de dependencias para los servicios 
            services.AddScoped<ILibroServices, LibroServices>();
            #endregion
        }
    }
}
