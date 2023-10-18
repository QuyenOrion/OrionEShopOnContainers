namespace OrionEShopOnContainer.Services.Ordering.API.Application.IntegrationEvents.Events;

public class OrderStartedIntegrationEvent
{
    public string UserId { get; init; }

    public OrderStartedIntegrationEvent(string userId)
        => UserId = userId;
}
