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
                    Description = "This api will be responsible for overall data distribution.",
                    Contact = new OpenApiContact
                    {
                        Name = "Sebastián Omar J.M.",
                        Email = "omarjoaquinminayasebastian@gmail.com"
                    }
                });

                options.DescribeAllParametersInCamelCase();
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