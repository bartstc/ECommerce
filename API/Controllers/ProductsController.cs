using API.Products.Dtos;
using Application.Products;
using Application.Products.Dtos;
using Application.Products.Exceptions;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers;

public class ProductsController : BaseApiController
{
    private readonly IMediator Mediator;

    public ProductsController(IMediator mediator)
    {
        Mediator = mediator;
    }

    [HttpGet]
    [SwaggerOperation("Get a list of all products")]
    [ProducesResponseType(typeof(List<ProductDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProducts()
    {
        var result = await Mediator.Send(new GetProducts.Query());
        return HandleResult(result, products => products.Select(p => p.ToDto()).ToList());
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

    [HttpPost]
    [SwaggerOperation("Add a new product")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddProduct([FromBody, SwaggerParameter("The product details")] CreateProductDto productDto)
    {
        var result = await Mediator.Send(new AddProduct.Command(productDto));
        return HandleResult(result);
    }

    [HttpPut("{id}")]
    [SwaggerOperation("Update an existing product by ID")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateProduct([FromRoute, SwaggerParameter("The product ID")] Guid id, [FromBody, SwaggerParameter("The updated product details")] CreateProductDto productDto)
    {
        var result = await Mediator.Send(new UpdateProduct.Command(ProductId.Of(id), productDto));
        if (!result.IsSuccess && result.Error.TypeOf<ProductNotFoundException>()) return NotFound(result.Error);
        return HandleResult(result);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation("Delete a product by ID")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteProduct([FromRoute, SwaggerParameter("The product ID")] Guid id)
    {
        var result = await Mediator.Send(new DeleteProduct.Command(ProductId.Of(id)));
        if (!result.IsSuccess && result.Error.TypeOf<ProductNotFoundException>()) return NotFound(result.Error);
        return HandleResult(result);
    }

    [HttpPatch("{id}/rate")]
    [SwaggerOperation("Rate a product by ID")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RateProduct([FromRoute, SwaggerParameter("The product ID")] Guid id, [FromBody, SwaggerParameter("The rating details")] RateProductDto rateProductDto)
    {
        var result = await Mediator.Send(new RateProduct.Command(ProductId.Of(id), rateProductDto));
        if (!result.IsSuccess && result.Error.TypeOf<ProductNotFoundException>()) return NotFound(result.Error);
        return HandleResult(result);
    }
}