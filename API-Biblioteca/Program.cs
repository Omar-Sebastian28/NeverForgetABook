using Bliblioteca.Infraestructura.Persistencia;
using Bliblioteca.Core.Aplication;
using API_Biblioteca.Extesions;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//Extension method.
builder.Services.AddPersistenceInfrastructure(builder.Configuration);
builder.Services.AddAplicationServices();
builder.Services.AddSwaggerExtension();
builder.Services.AddVersioningSwaggerExtension();
builder.Services.AddHealthChecks();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerExtension(app);
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseHealthChecks("/health");

app.MapControllers();

await app.RunAsync();
