using Application.Dtos;
using Application.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> GetProducts()
        {
            return await Mediator.Send(new List.Query());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(Guid id)
        {
            return await Mediator.Send(new Details.Query(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto productDto)
        {
            await Mediator.Send(new Create.Command(productDto));

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditProduct(Guid id, CreateProductDto productDto)
        {
            await Mediator.Send(new Edit.Command(id, productDto));

            return Ok();
        }
    }
}