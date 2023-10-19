namespace OrionEShopOnContainer.Services.Ordering.Domain.AggregatesModel.OrderAggregate;

public interface IOrderRepository
{
    Order Add(Order order);

    void Update(Order order);

    Task<Order> GetAsync(int orderId);
}