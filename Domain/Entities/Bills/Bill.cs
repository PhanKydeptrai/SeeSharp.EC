using Domain.Entities.Customers;
using Domain.Entities.Orders;
using Domain.Entities.ShippingInformations;

namespace Domain.Entities.Bills;
public sealed class Bill
{
    public BillId BillId { get; private set; } = null!;
    public OrderId OrderId { get; private set; } = null!;
    public CustomerId CustomerId { get; private set; } = null!;
    public DateTime CreatedDate { get; private set; } 
    public PaymentMethod PaymentMethod { get; private set; }
    public BillPaymentStatus BillPaymentStatus { get; private set; }
    public ShippingInformationId ShippingInformationId { get; private set; } = null!;

    //* Foreign key
    public Order Order { get; set; } = null!;
    public Customer Customer { get; set; } = null!;
    public ShippingInformation ShippingInformation { get; set; } = null!;


    private Bill(
        BillId billId,
        OrderId orderId,
        CustomerId customerId,
        DateTime createdDate,
        PaymentMethod paymentMethod,
        BillPaymentStatus billPaymentStatus,
        ShippingInformationId shippingInformationId)
    {
        BillId = billId;
        OrderId = orderId;
        CustomerId = customerId;
        CreatedDate = createdDate;
        PaymentMethod = paymentMethod;
        BillPaymentStatus = billPaymentStatus;
        ShippingInformationId = shippingInformationId;
    }

    public static Bill NewBill(
        OrderId orderId,
        CustomerId customerId,
        DateTime createdDate,
        PaymentMethod paymentMethod,
        BillPaymentStatus billPaymentStatus,
        ShippingInformationId shippingInformationId)
    {
        return new Bill(
            BillId.New(),
            orderId,
            customerId,
            createdDate,
            paymentMethod,
            billPaymentStatus,
            shippingInformationId);
    }
}
