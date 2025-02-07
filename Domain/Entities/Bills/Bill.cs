using Domain.Entities.Customers;
using Domain.Entities.Orders;
using Domain.Entities.ShippingInformations;

namespace Domain.Entities.Bills;
//NOTE: Create factory method
public class Bill
{
    public BillId BillId { get; set; } = null!;
    public OrderId OrderId { get; set; } = null!;
    public CustomerId CustomerId { get; set; } = null!;
    public DateTime CreatedDate { get; set; } 
    public PaymentMethod PaymentMethod { get; set; }
    public ShippingInformationId ShippingInformationId { get; set; } = null!;

    //* Foreign key
    public Order Order { get; set; } = null!;
    public Customer Customer { get; set; } = null!;
    public ShippingInformation ShippingInformation { get; set; } = null!;
}
