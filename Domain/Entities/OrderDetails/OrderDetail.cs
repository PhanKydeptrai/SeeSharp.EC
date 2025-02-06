using Domain.Entities.Orders;
using Domain.Entities.Products;

namespace Domain.Entities.OrderDetails;
//NOTE: Create factory method
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
}
