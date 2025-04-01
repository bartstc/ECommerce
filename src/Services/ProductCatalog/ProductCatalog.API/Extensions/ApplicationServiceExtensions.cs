using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using ECommerce.Core.Persistence;
using ProductCatalog.Infrastructure;
using ProductCatalog.Infrastructure.Projections;
using ProductCatalog.Application.Products;
using ProductCatalog.Domain;
using Ecommerce.Core.Infrastructure.EventStore;
using ProductCatalog.Infrastructure.Documents;

namespace ProductCatalog.API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddRouting(options => options.LowercaseUrls = true);
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddDbContext<DataContext>(opt => { opt.UseNpgsql(config.GetConnectionString("DefaultConnection")); });
        services.AddMarten(config, options =>
        {
            options.ConfigureProjections();
            options.ConfigureDocuments();
        });
        services.AddCors(opt =>
        {
            opt.AddPolicy("CorsPolicy",
                policy => { policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000"); });
        });
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ListProducts).Assembly));
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<AddProduct>();
        services.AddHttpContextAccessor();

        services.AddScoped<IEventStoreRepository<Product>, EventStoreRepository<Product>>();

        return services;
    }
}