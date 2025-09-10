using BIblioteca.Infraestructura.Share.Services;
using Bliblioteca.Core.Aplication.Interfaces;
using Bliblioteca.Core.Domain.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BIblioteca.Infraestructura.Share
{
    public static class ServicesRegistration
    {
        public static void AddShareLayerIoc(this IServiceCollection services, IConfiguration config) 
        {

            #region "Extraigo los valores del appSetting para pasarselos a la clase MailSettings."
            services.Configure<MailSettings>(config.GetSection("MailSettings"));
            #endregion

            #region "Inyeccion de dependencia."
            services.AddScoped<IEmailServices, EmailServices>();
            #endregion
        }
    }
}
