using Bliblioteca.Infraestructura.Persistencia;
using Bliblioteca.Core.Aplication;
using API_Biblioteca.Extesions;
using Biblioteca.Infraestructura.Identity;
using BIblioteca.Infraestructura.Share;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(opt => 
{
    opt.Filters.Add(new ProducesAttribute("application/json"));

}).ConfigureApiBehaviorOptions(opt => 
{
    opt.SuppressInferBindingSourcesForParameters = true; 
   // opt.SuppressMapClientErrors = true;

}).AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
var configuration = builder.Configuration;

//Extension method.
builder.Services.AddPersistenceInfrastructure(configuration);
builder.Services.AddAplicationServices();
builder.Services.AddSwaggerExtension();
builder.Services.AddVersioningSwaggerExtension();
builder.Services.AddHealthChecks();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddShareLayerIoc(configuration);
builder.Services.AddLayerIdentityForWebApp(configuration);

var app = builder.Build();
await app.Services.AddRunSeeds();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwaggerExtension(app);
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseHealthChecks("/health");  
app.MapControllers();

await app.RunAsync();
