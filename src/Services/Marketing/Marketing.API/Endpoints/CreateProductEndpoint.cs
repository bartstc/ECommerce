using Marketing.Application;
using Marketing.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Marketing.API.Endpoints;

public record CreateProductResponse(Guid Id);

public class CreateProductEndpoint
{
    public void RegisterEndpoint(RouteGroupBuilder group)
    {
        group.MapPost("",
                async Task<IResult> ([FromBody] CreateProductDto productDto, ISender sender) =>
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
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .WithOpenApi();
    }
}