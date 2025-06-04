using Domain.Entities.Customers;
using Domain.Entities.Feedbacks;
using Domain.Entities.Orders;
using Domain.Entities.ShippingInformations;
using Domain.Entities.Users;

namespace Domain.Entities.Bills;

public sealed class Bill
{
    public BillId BillId { get; private set; } = null!;
    public OrderId OrderId { get; private set; } = null!;
    public CustomerId CustomerId { get; private set; } = null!;
    public DateTime CreatedDate { get; private set; } 
    public PaymentMethod PaymentMethod { get; private set; }
    public BillPaymentStatus BillPaymentStatus { get; private set; }
    public FullName FullName { get; private set; } = FullName.Empty;
    public PhoneNumber PhoneNumber { get; private set; } = PhoneNumber.Empty;
    public SpecificAddress SpecificAddress { get; private set; } = SpecificAddress.Empty;
    public Province Province { get; private set; } = Province.Empty;
    public District District { get; private set; } = District.Empty;
    public Ward Ward { get; private set; } = Ward.Empty;
    public IsRated IsRated { get; private set; } = IsRated.NotRated;
    
    //* Foreign key
    public Order Order { get; set; } = null!;
    public Feedback? Feedback { get; set; } = null!;
    public Customer Customer { get; set; } = null!;


    private Bill(
        BillId billId,
        OrderId orderId,
        CustomerId customerId,
        DateTime createdDate,
        PaymentMethod paymentMethod,
        BillPaymentStatus billPaymentStatus)
    {
        BillId = billId;
        OrderId = orderId;
        CustomerId = customerId;
        CreatedDate = createdDate;
        PaymentMethod = paymentMethod;
        BillPaymentStatus = billPaymentStatus;
    }

    public static Bill NewBill(
        OrderId orderId,
        CustomerId customerId,
        DateTime createdDate,
        PaymentMethod paymentMethod,
        BillPaymentStatus billPaymentStatus)
    {
        return new Bill(
            BillId.New(),
            orderId,
            customerId,
            createdDate,
            paymentMethod,
            billPaymentStatus);
    }

    public void ChangeBillStatus(
        BillPaymentStatus billPaymentStatus, 
        PaymentMethod paymentMethod)
    {
        BillPaymentStatus = billPaymentStatus;
        PaymentMethod = paymentMethod;
    }

    public void MarkAsRated()
    {
        IsRated = IsRated.Rated;
    }
}
