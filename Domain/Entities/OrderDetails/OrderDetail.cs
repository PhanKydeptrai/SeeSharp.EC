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

    /// <summary>
    /// Create new OrderDetail
    /// </summary>
    /// <param name="orderId"></param>
    /// <param name="productId"></param>
    /// <param name="quantity"></param>
    /// <param name="productPrice"></param>
    /// <returns></returns>
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
    /// <summary>
    /// Create new OrderDetail from existing order detail (For message consumer)
    /// </summary>
    /// <param name="orderId"></param>
    /// <param name="productId"></param>
    /// <param name="quantity"></param>
    /// <param name="productPrice"></param>
    /// <returns></returns>
    public static OrderDetail FromExisting(
        OrderDetailId orderDetailId,
        OrderId orderId,
        ProductId productId,
        OrderDetailQuantity quantity,
        OrderDetailUnitPrice orderDetailUnitPrice)
    {
            
        return new OrderDetail(
            orderDetailId,
            orderId,
            productId,
            quantity,
            orderDetailUnitPrice);
    }

    /// <summary>
    /// Update quantity and unit price of order detail when add more product
    /// </summary>
    /// <param name="quantity"></param>
    /// <param name="productPrice"></param>
    public void UpdateQuantityAndProductPriceAfterAddMoreProduct(OrderDetailQuantity quantity, ProductPrice productPrice)
    {
        Quantity = OrderDetailQuantity.FromInt(Quantity.Value + quantity.Value);
        UnitPrice = OrderDetailUnitPrice.NewOrderDetailUnitPrice(productPrice.Value * Quantity.Value);
    }
    /// <summary>
    /// Update quantity and unit price of order detail 
    /// </summary>
    /// <param name="quantity"></param>
    /// <param name="productPrice"></param>
    public void ReCaculateUnitPrice(OrderDetailQuantity quantity, ProductPrice productPrice)
    {
        Quantity = quantity;
        UnitPrice = OrderDetailUnitPrice.NewOrderDetailUnitPrice(productPrice.Value * Quantity.Value);
    } 


    /// <summary>
    /// Replace unit price of order detail(Use for message consumer)
    /// </summary>
    /// <param name="productPrice"></param>
    public void ReplaceUnitPrice(OrderDetailUnitPrice unitPrice)
    {
        UnitPrice = unitPrice;
    }
    /// <summary>
    /// Replace quantity of order detail(Use for message consumer)
    /// </summary>
    /// <param name="quantity"></param>
    public void ReplaceQuantity(OrderDetailQuantity quantity)
    {
        Quantity = quantity;
    }
}
