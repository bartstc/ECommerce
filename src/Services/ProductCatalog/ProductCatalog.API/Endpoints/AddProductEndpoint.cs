using ProductCatalog.Application.Products.Dtos;

namespace ProductCatalog.API.Endpoints;

public class AddProductEndpoint
{
    public void RegisterEndpoint(RouteGroupBuilder group)
    {
        group.MapPost("",
                async Task<IResult> ([FromBody, SwaggerParameter("The product details")] AddProductDto productDto,
                    ISender sender) =>
                {
                    var result = await sender.Send(new AddProduct.Command(productDto));

                    return result.Match(
                        unit => Results.Created(),
                        error => Results.BadRequest(new { Message = error.Message }),
                        businessError => Results.BadRequest(new { Message = businessError.Message })
                    );
                })
            .WithName("AddProduct")
            .Produces(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithOpenApi();
    }
}