using Marketing.Infrastructure.Projections;

namespace Marketing.API.Endpoints;

public class GetProductEndpoint
{
    public void RegisterEndpoint(RouteGroupBuilder group)
    {
        group.MapGet("{id}",
                async Task<IResult> ([FromRoute, SwaggerParameter("The product ID")] Guid id, ISender sender) =>
                {
                    var result = await sender.Send(new GetProduct.Query(ProductId.Of(id)));

                    return result.Match(
                        product => Results.Ok(product),
                        notFound => Results.NotFound(new { Message = notFound.Message })
                    );
                })
            .WithName("GetProduct")
            .Produces<ProductDetails>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithOpenApi();
    }
}