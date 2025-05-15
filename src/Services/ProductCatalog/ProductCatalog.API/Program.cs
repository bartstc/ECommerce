using Marten;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProductCatalog.API.Extensions;
using ProductCatalog.Infrastructure;
using ProductCatalog.Infrastructure.Auth;
using Ecommerce.Core.Infrastructure.Middleware;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddControllers(opt =>
// {
//     var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
//     opt.Filters.Add(new AuthorizeFilter(policy));
// });
// builder.Services.AddIdentityServices(builder.Configuration);
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

// app.UseAuthentication();
// app.UseAuthorization();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

// Apply any pending migrations and seed the database with users and events
try
{
    var context = services.GetRequiredService<DataContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var documentSession = services.GetRequiredService<IDocumentSession>();
    await context.Database.MigrateAsync();
    await Seed.SeedData(userManager, documentSession);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}

app.Run();