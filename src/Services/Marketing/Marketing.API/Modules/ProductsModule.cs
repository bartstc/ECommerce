using Marketing.API.Endpoints;

namespace Marketing.API.Modules;

public class ProductsModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/products")
            .WithTags("Products");

        new GetProductEndpoint().RegisterEndpoint(group);
        new CreateProductEndpoint().RegisterEndpoint(group);
        new ArchiveProductEndpoint().RegisterEndpoint(group);
        new RateProductEndpoint().RegisterEndpoint(group);
    }
}