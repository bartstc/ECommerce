using ECommerce.Core.Application;
using Marketing.API.Dtos;
using Marketing.Application;
using Marketing.Application.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ProductCatalog.API.Controllers;

public class ProductsController : BaseApiController
{
    private readonly IMediator Mediator;

    public ProductsController(IMediator mediator)
    {
        Mediator = mediator;
    }

    [HttpGet("{id}")]
    [SwaggerOperation("Get details of a specific product by ID")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProduct([FromRoute, SwaggerParameter("The product ID")] Guid id)
    {
        var result = await Mediator.Send(new GetProduct.Query(ProductId.Of(id)));
        if (!result.IsSuccess && result.Error.TypeOf<ProductNotFoundException>()) return NotFound(result.Error);
        return HandleResult(result, product => product.ToDto());
    }

    // [HttpPost]
    // [SwaggerOperation("Create a new product")]
    // [ProducesResponseType(StatusCodes.Status201Created)]
    // [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    // [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    // public async Task<Results<Created, BadRequest<ErrorResponse>, Conflict<ErrorResponse>>> CreateProduct(
    //     [FromBody, SwaggerParameter("The product details")]
    //     CreateProductDto productDto)
    // {
    //     var result = await Mediator.Send(new CreateProduct.Command(productDto));
    //
    //     return result.Match<Results<Created, BadRequest<ErrorResponse>, Conflict<ErrorResponse>>>(
    //         unit => TypedResults.Created(),
    //         error => TypedResults.BadRequest(new ErrorResponse(error.Message)),
    //         businessError => TypedResults.BadRequest(new ErrorResponse(businessError.Message)),
    //         alreadyExists => TypedResults.Conflict(new ErrorResponse(alreadyExists.Message))
    //     );
    // }

    [HttpDelete("{id}")]
    [SwaggerOperation("Archive the product")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ArchiveProduct([FromRoute, SwaggerParameter("The product ID")] Guid id)
    {
        var result = await Mediator.Send(new ArchiveProduct.Command(ProductId.Of(id)));
        if (!result.IsSuccess && result.Error.TypeOf<ProductNotFoundException>()) return NotFound(result.Error);
        return HandleResult(result);
    }

    [HttpPatch("{id}/rate")]
    [SwaggerOperation("Rate the product")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RateProduct([FromRoute, SwaggerParameter("The product ID")] Guid id,
        [FromBody, SwaggerParameter("The rating details")]
        RateProductDto rateProductDto)
    {
        var result = await Mediator.Send(new RateProduct.Command(ProductId.Of(id), rateProductDto));
        if (!result.IsSuccess && result.Error.TypeOf<ProductNotFoundException>()) return NotFound(result.Error);
        return HandleResult(result);
    }
}