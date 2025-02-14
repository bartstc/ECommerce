using Application.Products;
using Application.Products.Dtos;
using Application.Products.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IMediator Mediator;

        public ProductsController(IMediator mediator)
        {
            Mediator = mediator;

        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var result = await Mediator.Send(new List.Query());
            return HandleResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            var result = await Mediator.Send(new Details.Query(id));
            if (result.Error is ProductNotFoundException) return NotFound(result.Error.Message);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto productDto)
        {
            var result = await Mediator.Send(new Create.Command(productDto));
            if (result.Error is FailedToCreateProductException) return BadRequest(result.Error.Message);
            return HandleResult(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditProduct(Guid id, CreateProductDto productDto)
        {
            var result = await Mediator.Send(new Edit.Command(id, productDto));
            if (result.Error is ProductNotFoundException) return NotFound(result.Error.Message);
            if (result.Error is FailedToUpdateProductException) return BadRequest(result.Error.Message);
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var result = await Mediator.Send(new Delete.Command(id));
            if (result.Error is ProductNotFoundException) return NotFound(result.Error.Message);
            if (result.Error is FailedToDeleteProductException) return BadRequest(result.Error.Message);
            return HandleResult(result);
        }

        [HttpPatch("{id}/rate")]
        public async Task<IActionResult> RateProduct(Guid id, RateProductDto rateProductDto)
        {
            var result = await Mediator.Send(new RateProduct.Command(id, rateProductDto));
            if (result.Error is ProductNotFoundException) return NotFound(result.Error.Message);
            return HandleResult(result);
        }
    }
}