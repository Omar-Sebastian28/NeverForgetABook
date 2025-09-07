using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;

namespace API_Biblioteca.Extesions
{
    public static class AppExtension
    {
        public static void UseSwaggerExtension(this IApplicationBuilder app, IEndpointRouteBuilder routeBuilder)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                var versionDescriptions = routeBuilder.DescribeApiVersions();
                if (versionDescriptions != null && versionDescriptions.Any())
                {
                    foreach (var apiVersion in versionDescriptions)
                    {
                        var url = $"{apiVersion.GroupName}/swagger.json";
                        var name = $"Biblioteca API - {apiVersion.GroupName.ToUpperInvariant()}";
                        options.SwaggerEndpoint(url, name);
                        Console.WriteLine($"{url} {name}");
                    }
                }
            });

        }
    }
}


