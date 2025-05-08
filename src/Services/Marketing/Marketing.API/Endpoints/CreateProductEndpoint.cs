namespace Marketing.API.Endpoints;

public class CreateProductEndpoint
{
    public void RegisterEndpoint(RouteGroupBuilder group)
    {
        group.MapPost("",
                async Task<IResult> ([FromBody, SwaggerParameter("The product details")] CreateProductDto productDto,
                    ISender sender) =>
                {
                    var result = await sender.Send(new CreateProduct.Command(productDto));

                    return result.Match(
                        unit => Results.Created(),
                        error => Results.BadRequest(new { Message = error.Message }),
                        businessError => Results.BadRequest(new { Message = businessError.Message }),
                        alreadyExists => Results.Conflict(new { Message = alreadyExists.Message })
                    );
                })
            .WithName("CreateProduct")
            .Produces(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithOpenApi();
    }
}