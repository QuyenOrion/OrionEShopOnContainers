using OrionEShopOnContainer.Services.Ordering.Domain.SeedWork;

namespace OrionEShopOnContainer.Services.Ordering.Domain.AggregatesModel.OrderAggregate;
public class OrderStatus : Entity
{
    public static OrderStatus Submitted = new OrderStatus(1, nameof(Submitted).ToLowerInvariant());
    public static OrderStatus AwaitingValidation = new OrderStatus(2, nameof(AwaitingValidation).ToLowerInvariant());
    public static OrderStatus StockConfirmed = new OrderStatus(3, nameof(StockConfirmed).ToLowerInvariant());
    public static OrderStatus Paid = new OrderStatus(4, nameof(Paid).ToLowerInvariant());
    public static OrderStatus Shipped = new OrderStatus(5, nameof(Shipped).ToLowerInvariant());
    public static OrderStatus Cancelled = new OrderStatus(6, nameof(Cancelled).ToLowerInvariant());

    private string _name;

    public OrderStatus(int id, string name)
    {
        Id = id;
        _name = name;
    }

    public string Name => _name;

    public static IEnumerable<OrderStatus> List() =>
          new[] { Submitted, AwaitingValidation, StockConfirmed, Paid, Shipped, Cancelled };
}