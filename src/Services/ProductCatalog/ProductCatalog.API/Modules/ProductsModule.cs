using ProductCatalog.API.Endpoints;

namespace ProductCatalog.API.Modules;

public class ProductsModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/products").WithTags("Products");
        
        new GetProductEndpoint().RegisterEndpoint(group);
        new ListProductsEndpoint().RegisterEndpoint(group);
        new AddProductEndpoint().RegisterEndpoint(group);
        new UpdateProductEndpoint().RegisterEndpoint(group);
        new DeleteProductEndpoint().RegisterEndpoint(group);
        new UpdatePriceEndpoint().RegisterEndpoint(group);
    }
}