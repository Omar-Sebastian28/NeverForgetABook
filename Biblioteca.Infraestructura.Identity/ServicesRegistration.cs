using Biblioteca.Infraestructura.Identity.Contexts;
using Biblioteca.Infraestructura.Identity.Entities;
using Biblioteca.Infraestructura.Identity.Seeds;
using Biblioteca.Infraestructura.Identity.Servicio;
using Bliblioteca.Core.Aplication.Interfaces;
using Bliblioteca.Core.Domain.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;


namespace Biblioteca.Infraestructura.Identity
{
    public static class ServicesRegistration
    {
        public static void AddLayerIdentityForWebApp(this IServiceCollection services, IConfiguration config)
        {
            GeneralConfiguration(services, config);

            #region Configuracion de Identity

            services.Configure<IdentityOptions>(opt =>
            {
                opt.Password.RequiredLength = 8;
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;

                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                opt.Lockout.MaxFailedAccessAttempts = 3;

                opt.User.RequireUniqueEmail = true;
                opt.SignIn.RequireConfirmedEmail = true;
            });


            services.AddIdentityCore<AppUser>()
                    .AddRoles<IdentityRole>()
                    .AddSignInManager()
                    .AddEntityFrameworkStores<IdentityContext>()
                    .AddTokenProvider<DataProtectorTokenProvider<AppUser>>(TokenOptions.DefaultProvider);

            services.Configure<DataProtectionTokenProviderOptions>(opt =>
            {
                opt.TokenLifespan = TimeSpan.FromHours(1);
            });


            //Lo más importante para que identity funcione correctamente.
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(opt => 
            {            
                opt.RequireHttpsMetadata = false;
                opt.SaveToken = false;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config["JwtSettings:Issuer"],
                    ValidAudience = config["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:SecretKey"] ?? "")),
                    ClockSkew = TimeSpan.Zero
                };
                opt.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = af =>
                    {
                        af.NoResult();
                        af.Response.StatusCode = 500;
                        af.Response.ContentType = "application/json";
                        var result = JsonSerializer.Serialize(new { error = "Ocurrio un error al procesar la petición", code = 500});
                        return  af.Response.WriteAsync(af.Exception.Message.ToString());
                    }, 
                    OnChallenge = c =>
                    {
                        c.HandleResponse();
                        c.Response.StatusCode = 401;
                        c.Response.ContentType = "application/json";
                        var result = JsonSerializer.Serialize(new { error = "You are not Authorized", code = 401});
                        return c.Response.WriteAsync(result);
                    },
                    OnForbidden = f =>
                    {
                        f.Response.StatusCode = 403;
                        f.Response.ContentType = "application/json";
                        var result = System.Text.Json.JsonSerializer.Serialize(new { error = "You are not authorized to access this resource" });
                        return f.Response.WriteAsync(result);
                    }
                };
            }).AddCookie(IdentityConstants.ApplicationScheme, opt => 
            {
                opt.ExpireTimeSpan = TimeSpan.FromMinutes(180);
            });
            #endregion


            #region servicio 
            services.AddScoped<IBaseAccountServices, BaseAccountServices>();
            services.AddScoped<IAccountServicesForWebApi, AccountServicesForWebApi>();
            #endregion
        }


        public static async Task AddRunSeeds(this IServiceProvider service)
        {
            using var scope = service.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await DefaultRoles.CreateDefaultRoles(RoleManager);
            await DefaultAdmin.CreateDefaultAdmin(userManager);
            await DefaultUser.CreateDefaultUser(userManager);
           
        }


        private static void GeneralConfiguration(IServiceCollection services, IConfiguration config) 
        {
            #region  Configuration Options
            services.Configure<JwtSettings>(config.GetSection("JwtSettings"));
            #endregion

            #region Context 
            if (config.GetValue<bool>("InMemoryDb"))
            {
                services.AddDbContext<IdentityContext>(opt => opt.UseInMemoryDatabase("WebApp"));
            }
            else
            {
                var connectionStrings = config.GetConnectionString("IdentityConnection");
                services.AddDbContext<IdentityContext>(                
                (servicesProvider, opt) =>
                {
                    opt.EnableSensitiveDataLogging();
                    opt.UseSqlServer(connectionStrings,
                     m => m.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName));
                },
                   contextLifetime : ServiceLifetime.Scoped,
                   optionsLifetime : ServiceLifetime.Scoped
                );          
            }
            #endregion
        }
    }
}
