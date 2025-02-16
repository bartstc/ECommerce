using API.Products.Dtos;
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
        var result = await Mediator.Send(new GetProducts.Query());
        return HandleResult(result, products => products.Select(p => p.ToDto()).ToList());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProduct(Guid id)
    {
        var result = await Mediator.Send(new GetProduct.Query(ProductId.Of(id)));
        if (!result.IsSuccess && result.Error.TypeOf<ProductNotFoundException>()) return NotFound(result.Error);
        return HandleResult(result, product => product.ToDto());
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddProduct(CreateProductDto productDto)
    {
        var result = await Mediator.Send(new AddProduct.Command(productDto));
        return HandleResult(result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateProduct(Guid id, CreateProductDto productDto)
    {
        var result = await Mediator.Send(new UpdateProduct.Command(ProductId.Of(id), productDto));
        if (!result.IsSuccess && result.Error.TypeOf<ProductNotFoundException>()) return NotFound(result.Error);
        return HandleResult(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var result = await Mediator.Send(new DeleteProduct.Command(ProductId.Of(id)));
        if (!result.IsSuccess && result.Error.TypeOf<ProductNotFoundException>()) return NotFound(result.Error);
        return HandleResult(result);
    }

    [HttpPatch("{id}/rate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RateProduct(Guid id, RateProductDto rateProductDto)
    {
        var result = await Mediator.Send(new RateProduct.Command(ProductId.Of(id), rateProductDto));
        if (!result.IsSuccess && result.Error.TypeOf<ProductNotFoundException>()) return NotFound(result.Error);
        return HandleResult(result);
    }
}
