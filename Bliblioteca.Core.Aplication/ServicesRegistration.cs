using Bliblioteca.Core.Aplication.Interfaces;
using Bliblioteca.Core.Aplication.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Bliblioteca.Core.Aplication
{ 
    public static class ServicesRegistration
    {
        public static void AddAplicationServices(this IServiceCollection services)
        {
            #region Configurando AutoMapper y MediatR
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(opt => opt.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            #endregion


            #region Configurando inyeccion de dependencias para los servicios 
            services.AddScoped<ILibroServices, LibroServices>();
            #endregion
        }
    }
}
