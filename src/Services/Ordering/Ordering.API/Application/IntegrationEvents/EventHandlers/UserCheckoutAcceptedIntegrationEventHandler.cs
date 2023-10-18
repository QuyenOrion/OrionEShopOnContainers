namespace OrionEShopOnContainer.Services.Ordering.API.Application.IntegrationEvents.EventHandlers;

public class UserCheckoutAcceptedIntegrationEventHandler
{
    private readonly ILogger<UserCheckoutAcceptedIntegrationEventHandler> _logger;
    private readonly IMediator _mediator;

    public UserCheckoutAcceptedIntegrationEventHandler(ILogger<UserCheckoutAcceptedIntegrationEventHandler> logger, IMediator mediator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task Handle(UserCheckoutAcceptedIntegrationEvent @event)
    {
        using (_logger.BeginScope(new List<KeyValuePair<string, object>> { new("IntegrationEventContext", @event.Id) }))
        {
            _logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.Id, @event);

            if (@event.RequestId != Guid.Empty)
            {
                using (_logger.BeginScope(new List<KeyValuePair<string, object>> { new("IdentifiedCommandId", @event.RequestId) }))
                {
                    var createOrderCommand = new CreateOrderCommand(@event.Basket.Items, @event.UserId, @event.UserName, 
                                                @event.City, @event.Street, @event.State, @event.Country, @event.ZipCode,
                                                @event.CardNumber, @event.CardHolderName, @event.CardExpiration, 
                                                @event.CardSecurityNumber, @event.CardTypeId);

                    var requestCreateOrder = new IdentifiedCommand<CreateOrderCommand, bool>(createOrderCommand, @event.RequestId);

                    _logger.LogInformation(
                        "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                        requestCreateOrder.GetGenericTypeName(),
                        nameof(requestCreateOrder.Id),
                        requestCreateOrder.Id,
                        requestCreateOrder);

                    var result = await _mediator.Send(requestCreateOrder);

                    if (result)
                        _logger.LogInformation("CreateOrderCommand suceeded - RequestId: {RequestId}", @event.RequestId);
                    else
                        _logger.LogWarning("CreateOrderCommand failed - RequestId: {RequestId}", @event.RequestId);
                }
            }
            else
            {
                _logger.LogWarning("Invalid IntegrationEvent - RequestId is missing - {@IntegrationEvent}", @event);
            }
        }
    }
}
