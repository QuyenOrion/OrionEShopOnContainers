using MediatR;
using OrionEShopOnContainer.Services.Ordering.Domain.AggregatesModel.OrderAggregate;

namespace OrionEShopOnContainer.Services.Ordering.Domain.Events;
public class OrderAwaitingValidationDomainEvent : INotification
{
    public int OrderId { get; }

    public IEnumerable<OrderItem> OrderItems { get; }

    public OrderAwaitingValidationDomainEvent(int orderId,
        IEnumerable<OrderItem> orderItems)
    {
        OrderId = orderId;
        OrderItems = orderItems;
    }
}
