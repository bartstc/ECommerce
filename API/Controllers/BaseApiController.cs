using ECommerce.Core.Application;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    protected ActionResult HandleResult<T>(Result<T> result)
    {
        if (result == null) return NotFound();
        if (result.IsSuccess && result.Value != null) return Ok(result.Value);
        if (result.IsSuccess && result.Value == null) return NotFound();
        return BadRequest(result.Error);
    }

    protected ActionResult HandleResult<T, TResult>(Result<T> result, Func<T, TResult> map)
    {
        if (result == null) return NotFound();
        if (result.IsSuccess && result.Value != null) return Ok(map(result.Value));
        if (result.IsSuccess && result.Value == null) return NotFound();
        return BadRequest(result.Error);
    }
}