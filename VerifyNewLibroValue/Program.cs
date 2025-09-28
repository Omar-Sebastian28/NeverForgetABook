using Bliblioteca.Infraestructura.Persistencia;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Bliblioteca.Core.Aplication;
using Biblioteca.Infraestructura.Identity;
using BIblioteca.Infraestructura.Share;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

//Extension method.
builder.Services.AddPersistenceInfrastructure(builder.Configuration);
builder.Services.AddAplicationServices();
builder.Services.AddHealthChecks();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddShareLayerIoc(builder.Configuration);
builder.Services.AddLayerIdentityForWebApp(builder.Configuration);


await builder.Build().RunAsync();
