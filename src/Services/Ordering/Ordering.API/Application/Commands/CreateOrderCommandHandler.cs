namespace OrionEShopOnContainer.Services.Ordering.API.Application.Commands;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, bool>
{
    private readonly ILogger<CreateOrderCommandHandler> _logger;

    public CreateOrderCommandHandler(ILogger<CreateOrderCommandHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task<bool> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var address = new Address(request.Street, request.City, request.State, request.Country, request.ZipCode);
        var order = new Order(request.UserId, request.UserId, address, request.CardTypeId, request.CardNumber,
                        request.CardSecurityNumber, request.CardHolderName, request.CardExpiration);

        foreach (var item in request.OrderItems)
        {
            order.AddOrderItem(item.ProductId, item.ProductName, item.UnitPrice, item.Discount, item.PictureUrl, item.Units);
        }

        _logger.LogInformation("Creating Order - Order: {@Order}", order);

        return Task.FromResult(true);
    }
}