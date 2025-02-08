using Application.Stores;
using Application.Stores.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class StoresController : BaseApiController
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStore(Guid id)
        {
            var result = await Mediator.Send(new Details.Query(id));
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStore(CreateStoreDto storeDto)
        {
            var result = await Mediator.Send(new Create.Command(storeDto));
            return HandleResult(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditStore(Guid id, CreateStoreDto storeDto)
        {
            var result = await Mediator.Send(new Edit.Command(id, storeDto));
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStore(Guid id)
        {
            var result = await Mediator.Send(new Delete.Command(id));
            return HandleResult(result);
        }
    }
}