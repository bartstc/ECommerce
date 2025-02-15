using Application.Products;
using Application.Products.Dtos;
using Application.Products.Exceptions;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProductsController : BaseApiController
{
    private readonly IMediator Mediator;

    public ProductsController(IMediator mediator)
    {
        Mediator = mediator;

    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ProductDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProducts()
    {
        var result = await Mediator.Send(new List.Query());
        return HandleResult(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProduct(Guid id)
    {
        var result = await Mediator.Send(new Details.Query(id));
        if (result.Error is ProductNotFoundException) return NotFound(result.Error.Message);
        return HandleResult(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddProduct(CreateProductDto productDto)
    {
        var result = await Mediator.Send(new AddProduct.Command(productDto));
        if (result.Error is FailedToCreateProductException) return BadRequest(result.Error.Message);
        return HandleResult(result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateProduct(Guid id, CreateProductDto productDto)
    {
        var result = await Mediator.Send(new UpdateProduct.Command(ProductId.Of(id), productDto));
        if (result.Error is ProductNotFoundException) return NotFound(result.Error.Message);
        if (result.Error is FailedToUpdateProductException) return BadRequest(result.Error.Message);
        return HandleResult(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var result = await Mediator.Send(new DeleteProduct.Command(ProductId.Of(id)));
        if (result.Error is ProductNotFoundException) return NotFound(result.Error.Message);
        if (result.Error is FailedToDeleteProductException) return BadRequest(result.Error.Message);
        return HandleResult(result);
    }

    [HttpPatch("{id}/rate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RateProduct(Guid id, RateProductDto rateProductDto)
    {
        var result = await Mediator.Send(new RateProduct.Command(ProductId.Of(id), rateProductDto));
        if (result.Error is ProductNotFoundException) return NotFound(result.Error.Message);
        if (result.Error is FailedToRateProductException) return BadRequest(result.Error.Message);
        return HandleResult(result);
    }
}
