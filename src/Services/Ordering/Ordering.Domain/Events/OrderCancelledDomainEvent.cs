namespace OrionEShopOnContainer.Services.Ordering.Domain.Events;

public class OrderCancelledDomainEvent : INotification
{
    public Order Order { get; set; }

    public OrderCancelledDomainEvent(Order order)
    {
        this.Order = order;
    }
}
