using FluentValidation;
using FluentValidation.AspNetCore;
using ECommerce.Core.Persistence;
using Ecommerce.Core.Infrastructure.EventStore;
using Marketing.Infrastructure.Projections;
using Marketing.Application;

namespace Marketing.API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddRouting(options => options.LowercaseUrls = true);
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddMarten(config, options => options.ConfigureProjections());
        services.AddCors(opt =>
        {
            opt.AddPolicy("CorsPolicy", policy =>
            {
                policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000");
            });
        });
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetProduct).Assembly));
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<CreateProduct>();
        services.AddHttpContextAccessor();

        services.AddScoped<IEventStoreRepository<Product>, EventStoreRepository<Product>>();

        return services;
    }
}

