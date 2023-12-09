using Basket.API.Repositories;

namespace OrionEShopOnContainers.Services.Basket.API.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
[Authorize]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository _repository;

    public BasketController(IBasketRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerBasket>> GetBasketByIdAsync(string id)
    {
        var basket = await _repository.GetBasketAsync(id);

        return basket != null ? (ActionResult<CustomerBasket>)Ok(basket) : (ActionResult<CustomerBasket>)NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<CustomerBasket>> UpdateBasketAsync([FromBody] CustomerBasket value)
    {
        return Ok(await _repository.UpdateBasketAsync(value));
    }
}
