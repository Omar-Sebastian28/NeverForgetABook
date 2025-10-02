using Asp.Versioning;
using Microsoft.OpenApi.Models;

namespace API_Biblioteca.Extesions
{
    public static class ServicesRegistration
    {
        public static void AddSwaggerExtension(this IServiceCollection services)
        {         
            services.AddSwaggerGen(options =>
            {
                List<string> xmlsFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", searchOption: SearchOption.TopDirectoryOnly).ToList();
                xmlsFiles.ForEach(xmlsFiles => options.IncludeXmlComments(xmlsFiles));

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1.0",
                    Title = "Biblioteca API",
                    Description = "This api will be responsible for overall data distribution.",
                    Contact = new OpenApiContact
                    {
                        Name = "Sebastián Omar J.M.",
                        Email = "omarjoaquinminayasebastian@gmail.com"
                    }
                });

                options.SwaggerDoc("v2", new OpenApiInfo
                {
                    Version = "v2.0",
                    Title = "Biblioteca API",
                    Description = "This api (v2), will be responsible for overall data distribution.",
                    Contact = new OpenApiContact
                    {
                        Name = "Sebastián Omar J.M.",
                        Email = "omarjoaquinminayasebastian@gmail.com"
                    }
                });

                options.DescribeAllParametersInCamelCase();
                options.EnableAnnotations();

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Input your Bearer token in this format 'Bearer {your token here}' to access this API"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },new List<string>()
                    }
                });
            });
        }


        public static void AddVersioningSwaggerExtension(this IServiceCollection services)
        {
            services.AddApiVersioning(opt =>
            {
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.ReportApiVersions = true;
                opt.ApiVersionReader = ApiVersionReader.Combine
                (
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("X-Api-Version")
                );
            }).AddApiExplorer(opt => 
            {
                opt.GroupNameFormat = "'v'VVV"; // v1, v2...
                opt.SubstituteApiVersionInUrl = true;
            });
        }
    }
}