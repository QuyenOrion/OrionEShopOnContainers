using MediatR;
using OrionEShopOnContainer.Services.Ordering.Domain.AggregatesModel.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrionEShopOnContainer.Services.Ordering.Domain.Events
{
    public class OrderCancelledDomainEvent : INotification
    {
        public Order Order { get; set; }

        public OrderCancelledDomainEvent(Order order) { this.Order = order; }
    }
}
