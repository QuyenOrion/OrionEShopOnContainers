using MediatR;
using OrionEShopOnContainer.Services.Ordering.Domain.AggregatesModel.OrderAggregate;

namespace OrionEShopOnContainer.Services.Ordering.Domain.Events
{
    public class OrderShippedDomainEvent : INotification
    {
        public Order Order { get; }

        public OrderShippedDomainEvent(Order order)
        {
            Order = order;
        }
    }
}
