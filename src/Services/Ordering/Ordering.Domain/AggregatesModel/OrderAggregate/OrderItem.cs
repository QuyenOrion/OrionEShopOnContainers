using System;

namespace OrionEShopOnContainer.Services.Ordering.Domain.AggregatesModel.OrderAggregate;
public class OrderItem : Entity
{
    private int _id;
    private string _productName;
    private decimal _unitPrice;
    private int _units;
    private string _pictureUrl;
    private decimal _discount;

    public int ProductId { get; private set; }

    public OrderItem(int productId, string productName, decimal unitPrice, int units, string pictureUrl, decimal discount)
    {
        if (units <= 0)
        {
            throw new Exception("Invalid number of units");
        }

        if ((unitPrice * units) < discount)
        {
            throw new Exception("The total of order item is lower than applied discount");
        }

        ProductId = productId;

        _productName = productName ?? throw new ArgumentNullException(nameof(productName));
        _unitPrice = unitPrice;
        _units = units;
        _pictureUrl = pictureUrl ?? throw new ArgumentNullException(nameof(pictureUrl));
        _discount = discount;
    }

    public decimal UnitPrice => _unitPrice;

    public int Units => _units;

    public decimal GetCurrentDiscount() => _discount;

    public void SetNewDiscount(decimal discount)
    {
        if (discount < 0)
            throw new Exception("Discount is not valid");

        _discount = discount;
    }

    public void AddUnits(int units)
    {
        if (units <= 0)
            throw new Exception("Invalid units");

        _units += units;
    }
}
