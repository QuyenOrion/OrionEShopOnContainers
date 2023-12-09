using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OrionEShopOnContainer.Services.Ordering.API.Controllers;

[Route("identity")]
[Authorize]
public class IdentityController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
    }

    [HttpGet]
    [Route("{id}")]
    public IActionResult Get(int id)
    {
        return Ok(id);
    }
}