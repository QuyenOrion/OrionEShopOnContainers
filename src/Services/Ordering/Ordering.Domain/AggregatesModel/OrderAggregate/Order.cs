using System;
using System.Collections.Generic;
using OrionEShopOnContainer.Services.Ordering.Domain.Events;
using OrionEShopOnContainer.Services.Ordering.Domain.SeedWork;

namespace OrionEShopOnContainer.Services.Ordering.Domain.AggregatesModel.OrderAggregate;
public class Order : Entity, IAggregateRoot
{
    private readonly DateTime _orderDate;

    public Address Address { get; private set; }

    private int? _buyerId;
    public int? GetBuyerId => _buyerId;

    private readonly List<OrderItem> _orderItems;
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public OrderStatus OrderStatus { get; private set; }
    private int _orderStatusId;

    private int? _paymentMethodId;

    private string _description;

    private Order()
    {
        _orderItems = new List<OrderItem>();
    }

    public Order(string userId, string userName, Address address, int cardTypeId, string cardNumber, string cardSecurityNumber,
            string cardHolderName, DateTime cardExpiration, int? buyerId = null, int? paymentMethodId = null)
      : this()
    {
        _orderDate = DateTime.UtcNow;
        Address = address;
        _buyerId = buyerId;
        _paymentMethodId = paymentMethodId;
        _orderStatusId = OrderStatus.Submitted.Id;

        AddOrderStartedDomainEvent(userId, userName, cardTypeId, cardNumber,
                                   cardSecurityNumber, cardHolderName, cardExpiration);
    }

    private void AddOrderStartedDomainEvent(string userId, string userName, int cardTypeId, string cardNumber, string cardSecurityNumber, string cardHolderName, DateTime cardExpiration)
    {
        var orderStartedDomainEvent = new OrderStartedDomainEvent(this, userId, userName, cardTypeId,
                                                                    cardNumber, cardSecurityNumber,
                                                                    cardHolderName, cardExpiration);

        this.AddDomainEvent(orderStartedDomainEvent);
    }

    public void AddOrderItem(int productId, string productName, decimal unitPrice, decimal discount, string pictureUrl, int units)
    {
        var existingOrderForProduct = _orderItems.Find(o => o.ProductId == productId);

        if (existingOrderForProduct != null)
        {
            if (discount > existingOrderForProduct.GetCurrentDiscount())
                existingOrderForProduct.SetNewDiscount(discount);

            existingOrderForProduct.AddUnits(units);
        }
        else
        {
            var orderItem = new OrderItem(productId, productName, unitPrice, units, pictureUrl, discount);
            _orderItems.Add(orderItem);
        }
    }

    public void SetPaymentId(int paymentId)
    {
        _paymentMethodId = paymentId;
    }

    public void SetBuyerId(int id)
    {
        _buyerId = id;
    }

    public void SetCancelledStatus()
    {
        if (_orderStatusId == OrderStatus.Paid.Id
        || _orderStatusId == OrderStatus.Shipped.Id)
        {
            StatusChangeException(OrderStatus.Cancelled);
        }

        _orderStatusId = OrderStatus.Cancelled.Id;
        _description = $"Order was cancelled. ({DateTime.UtcNow})";
        AddDomainEvent(new OrderCancelledDomainEvent(this));
    }

    public void SetShippedStatus()
    {
        if (_orderStatusId == OrderStatus.Paid.Id)
        {
            _orderStatusId = OrderStatus.Shipped.Id;
            AddDomainEvent(new OrderShippedDomainEvent(this));
            return;
        }

        StatusChangeException(OrderStatus.Shipped);
    }

    public void SetPaidStatus()
    {
        if (_orderStatusId == OrderStatus.Submitted.Id)
        {
            _orderStatusId = OrderStatus.Paid.Id;
            AddDomainEvent(new OrderStatusChangedToPaidDomainEvent(Id, _orderItems));
            return;
        }

        StatusChangeException(OrderStatus.Paid);
    }

    public void SetAwaitingValidationStatus()
    {
        if (_orderStatusId == OrderStatus.Submitted.Id)
        {
            _orderStatusId = OrderStatus.AwaitingValidation.Id;
            AddDomainEvent(new OrderStatusChangedToAwaitingValidationDomainEvent(Id, _orderItems));
            return;
        }

        StatusChangeException(OrderStatus.AwaitingValidation);
    }

    public void SetStockConfirmedStatus()
    {
        if (_orderStatusId == OrderStatus.AwaitingValidation.Id)
        {
            AddDomainEvent(new OrderStatusChangedToStockConfirmedDomainEvent(Id));

            _orderStatusId = OrderStatus.StockConfirmed.Id;
            _description = "All the items were confirmed with available stock.";
        }
    }

    private void StatusChangeException(OrderStatus orderStatusToChange)
    {
        throw new Exception($"Is not possible to change the order status from {OrderStatus.Name} to {orderStatusToChange.Name}");
    }

    public decimal GetTotal()
    {
        decimal totalPrice = 0;

        foreach (var item in _orderItems)
        {
            totalPrice += item.UnitPrice * item.Units;
        }

        return totalPrice;
    }
}
