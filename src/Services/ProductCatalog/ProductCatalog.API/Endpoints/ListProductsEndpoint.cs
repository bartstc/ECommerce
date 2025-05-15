using ProductCatalog.API.Products.Dtos;

namespace ProductCatalog.API.Endpoints;

public class ListProductsEndpoint
{
    public void RegisterEndpoint(RouteGroupBuilder group)
    {
        group.MapGet("",
                async Task<IResult> (ISender sender) =>
                {
                    var result = await sender.Send(new ListProducts.Query());

                    return result.Match(
                        products => Results.Ok(products.Select(p => p.ToDto()))
                    );
                })
            .WithName("ListProducts")
            .Produces<List<ProductDto>>(StatusCodes.Status200OK)
            .WithOpenApi();
    }
}