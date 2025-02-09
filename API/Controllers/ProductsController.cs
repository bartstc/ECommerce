using Application.Products;
using Application.Products.Dtos;
using Domain.Errors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
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
            if (result.Error == ProductsError.ProductNotFound) return NotFound(result.Error);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto productDto)
        {
            var result = await Mediator.Send(new Create.Command(productDto));
            if (result.Error == ProductsError.FailedToCreateProduct) return BadRequest(result.Error);
            return HandleResult(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditProduct(Guid id, CreateProductDto productDto)
        {
            var result = await Mediator.Send(new Edit.Command(id, productDto));
            if (result.Error == ProductsError.ProductNotFound) return NotFound(result.Error);
            if (result.Error == ProductsError.FailedToUpdateProduct) return BadRequest(result.Error);
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var result = await Mediator.Send(new Delete.Command(id));
            if (result.Error == ProductsError.ProductNotFound) return NotFound(result.Error);
            if (result.Error == ProductsError.FailedToDeleteProduct) return BadRequest(result.Error);
            return HandleResult(result);
        }

        [HttpPatch("{id}/rate")]
        public async Task<IActionResult> RateProduct(Guid id, RateProductDto rateProductDto)
        {
            var result = await Mediator.Send(new RateProduct.Command(id, rateProductDto));
            if (result.Error == ProductsError.ProductNotFound) return NotFound(result.Error);
            if (result.Error == ProductsError.StoreNotFound) return BadRequest(result.Error);
            return HandleResult(result);
        }
    }
}