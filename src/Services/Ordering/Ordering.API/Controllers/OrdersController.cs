using Microsoft.AspNetCore.Mvc;

namespace OrionEShopOnContainer.Services.Ordering.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly ILogger<OrdersController> _logger;
    private readonly ILogger<UserCheckoutAcceptedIntegrationEventHandler> _loggerTest;
    private readonly IMediator _mediator;

    public OrdersController(ILogger<OrdersController> logger, ILogger<UserCheckoutAcceptedIntegrationEventHandler> loggerTest, IMediator mediator)
    {
        _logger = logger;
        _loggerTest = loggerTest;
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult> Index()
    {
        var handler = new UserCheckoutAcceptedIntegrationEventHandler(_loggerTest, _mediator);
        await handler.Handle(new UserCheckoutAcceptedIntegrationEvent("userId", "userName", "city", "street",
            "state", "country", "zipCode", "CardNumber", "cardHolderName", DateTime.Now, "cardSecurityNumber",
            1, "buyer", Guid.NewGuid(),
            new CustomerBasket("buyer",
            new List<BasketItem> { new BasketItem {
                Id = "1",
                ProductId = 2,
                PictureUrl = "picture url",
                ProductName = "product name",
                Quantity = 3,
                UnitPrice = 10,
        } })));

        return Ok();
    }
}