using ProductCatalog.Application.Products.Dtos;

namespace ProductCatalog.API.Endpoints;

public class UpdateProductEndpoint
{
    public void RegisterEndpoint(RouteGroupBuilder group)
    {
        group.MapPut("{id}",
                async Task<IResult> ([FromRoute, SwaggerParameter("The product ID")] Guid id,
                    [FromBody] UpdateProductDto productDto, ISender sender) =>
                {
                    var result = await sender.Send(new UpdateProduct.Command(ProductId.Of(id), productDto));

                    return result.Match(
                        unit => Results.NoContent(),
                        notFound => Results.NotFound(new { Message = notFound.Message }),
                        businessError => Results.BadRequest(new { Message = businessError.Message }),
                        error => Results.BadRequest(new { Message = error.Message })
                    );
                })
            .WithName("UpdateProduct")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithOpenApi();
    }
}