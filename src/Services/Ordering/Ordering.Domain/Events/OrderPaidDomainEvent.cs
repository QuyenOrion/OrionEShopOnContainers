using MediatR;
using OrionEShopOnContainer.Services.Ordering.Domain.AggregatesModel.OrderAggregate;

namespace OrionEShopOnContainer.Services.Ordering.Domain.Events
{
    public class OrderPaidDomainEvent
    : INotification
    {
        public int OrderId { get; }
        public IEnumerable<OrderItem> OrderItems { get; }

        public OrderPaidDomainEvent(int orderId,
            IEnumerable<OrderItem> orderItems)
        {
            OrderId = orderId;
            OrderItems = orderItems;
        }
    }
}
