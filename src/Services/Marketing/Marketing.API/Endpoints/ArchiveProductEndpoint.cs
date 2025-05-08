namespace Marketing.API.Endpoints;

public class ArchiveProductEndpoint
{
    public void RegisterEndpoint(RouteGroupBuilder group)
    {
        group.MapDelete("{id}",
                async Task<IResult> ([FromRoute, SwaggerParameter("The product ID")] Guid id, ISender sender) =>
                {
                    var result = await sender.Send(new ArchiveProduct.Command(ProductId.Of(id)));

                    return result.Match(
                        unit => Results.NoContent(),
                        error => Results.BadRequest(new { Message = error.Message }),
                        businessError => Results.BadRequest(new { Message = businessError.Message }),
                        notFound => Results.NotFound(new { Message = notFound.Message })
                    );
                })
            .WithName("ArchiveProduct")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithOpenApi();
    }
}