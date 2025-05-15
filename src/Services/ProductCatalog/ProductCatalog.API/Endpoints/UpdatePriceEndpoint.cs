using ProductCatalog.Application.Products.Dtos;

namespace ProductCatalog.API.Endpoints;

public class UpdatePriceEndpoint
{
    public void RegisterEndpoint(RouteGroupBuilder group)
    {
        group.MapPatch("{id}/price",
                async Task<IResult> ([FromRoute, SwaggerParameter("The product ID")] Guid id,
                    [FromBody] UpdatePriceDto updatePriceDto, ISender sender) =>
                {
                    var result = await sender.Send(new UpdatePrice.Command(ProductId.Of(id), updatePriceDto));

                    return result.Match(
                        unit => Results.NoContent(),
                        notFound => Results.NotFound(new { Message = notFound.Message }),
                        businessError => Results.BadRequest(new { Message = businessError.Message }),
                        error => Results.BadRequest(new { Message = error.Message })
                    );
                })
            .WithName("UpdateProductPrice")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithOpenApi();
    }
}