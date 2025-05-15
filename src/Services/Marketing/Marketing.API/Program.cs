using Marten;
using Microsoft.OpenApi.Models;
using Marketing.API.Extensions;
using Ecommerce.Core.Infrastructure.Middleware;
using Marketing.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarter();
builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ECommerce API", Version = "v1" });
    c.EnableAnnotations(); // Enable annotations
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.MapCarter();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

// Seed the database with events
try
{
    var documentSession = services.GetRequiredService<IDocumentSession>();
    await Seed.SeedData(documentSession);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}

app.Run();