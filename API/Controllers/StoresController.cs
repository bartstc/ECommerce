using Application.Stores;
using Application.Stores.Dtos;
using Domain.Errors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class StoresController : BaseApiController
    {
        private readonly IMediator Mediator;

        public StoresController(IMediator mediator)
        {
            Mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStore(Guid id)
        {
            var result = await Mediator.Send(new Details.Query(id));
            if (result.Error == StoresError.StoreNotFound) return NotFound(result.Error);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStore(CreateStoreDto storeDto)
        {
            var result = await Mediator.Send(new Create.Command(storeDto));
            if (result.Error == StoresError.FailedToCreateStore) return BadRequest(result.Error);
            return HandleResult(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditStore(Guid id, CreateStoreDto storeDto)
        {
            var result = await Mediator.Send(new Edit.Command(id, storeDto));
            if (result.Error == StoresError.StoreNotFound) return NotFound(result.Error);
            if (result.Error == StoresError.FailedToUpdateStore) return BadRequest(result.Error);
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStore(Guid id)
        {
            var result = await Mediator.Send(new Delete.Command(id));
            if (result.Error == StoresError.StoreNotFound) return NotFound(result.Error);
            return HandleResult(result);
        }
    }
}