using Domain.Entities.Orders;
using Domain.Entities.Products;

namespace Domain.Entities.OrderDetails;
public sealed class OrderDetail
{
    public OrderDetailId OrderDetailId { get; private set; } = null!;
    public OrderId OrderId { get; private set; } = null!;
    public ProductId ProductId { get; private set; } = null!;
    public OrderDetailQuantity Quantity { get; private set; } = null!;
    public OrderDetailUnitPrice UnitPrice { get; private set; } = null!; 
    //* Foreign Key
    public Order? Order { get; set; } = null!;
    public Product? Product { get; set; } = null!;

    private OrderDetail(
        OrderDetailId orderDetailId,
        OrderId orderId,
        ProductId productId,
        OrderDetailQuantity quantity,
        OrderDetailUnitPrice unitPrice)
    {
        OrderDetailId = orderDetailId;
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public static OrderDetail NewOrderDetail(
        OrderId orderId,
        ProductId productId,
        OrderDetailQuantity quantity,
        ProductPrice productPrice)
    {
        
        var unitPrice = OrderDetailUnitPrice
            .NewOrderDetailUnitPrice(productPrice.Value * quantity.Value);
            
        return new OrderDetail(
            OrderDetailId.New(),
            orderId,
            productId,
            quantity,
            unitPrice);
    }
    public void UpdateQuantityProductPrice(OrderDetailQuantity quantity, ProductPrice productPrice)
    {
        Quantity = OrderDetailQuantity.FromInt(Quantity.Value + quantity.Value);
        UnitPrice = OrderDetailUnitPrice.NewOrderDetailUnitPrice(productPrice.Value * Quantity.Value);
    }
}
