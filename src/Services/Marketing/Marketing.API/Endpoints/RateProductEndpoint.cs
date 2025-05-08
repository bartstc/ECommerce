namespace Marketing.API.Endpoints;

public class RateProductEndpoint
{
    public void RegisterEndpoint(RouteGroupBuilder group)
    {
        group.MapPatch("{id}/rate",
                async Task<IResult> ([FromRoute, SwaggerParameter("The product ID")] Guid id,
                    [FromBody] RateProductDto rateProductDto, ISender sender) =>
                {
                    var result = await sender.Send(new RateProduct.Command(ProductId.Of(id), rateProductDto));

                    return result.Match(
                        unit => Results.NoContent(),
                        error => Results.BadRequest(new { Message = error.Message }),
                        businessError => Results.BadRequest(new { Message = businessError.Message }),
                        notFound => Results.NotFound(new { Message = notFound.Message })
                    );
                })
            .WithName("RateProduct")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithOpenApi();
    }
}