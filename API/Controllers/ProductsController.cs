using Application.Products;
using Application.Products.Dtos;
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
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto productDto)
        {
            var result = await Mediator.Send(new Create.Command(productDto));
            return HandleResult(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditProduct(Guid id, CreateProductDto productDto)
        {
            var result = await Mediator.Send(new Edit.Command(id, productDto));
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var result = await Mediator.Send(new Delete.Command(id));
            return HandleResult(result);
        }
    }
}