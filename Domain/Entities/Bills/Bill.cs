using Domain.Entities.BillDetails;
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
    public BillPaymentStatus BillPaymentStatus { get; private set; }
    public FullName FullName { get; private set; } = FullName.Empty;
    public PhoneNumber PhoneNumber { get; private set; } = PhoneNumber.Empty;
    public Email Email { get; private set; } = Email.Empty;
    public SpecificAddress SpecificAddress { get; private set; } = SpecificAddress.Empty;
    public Province Province { get; private set; } = Province.Empty;
    public District District { get; private set; } = District.Empty;
    public Ward Ward { get; private set; } = Ward.Empty;
    public IsRated IsRated { get; private set; } = IsRated.NotRated;
    
    //* Foreign key
    public Order Order { get; set; } = null!;
    public Feedback? Feedback { get; set; } = null!;
    public Customer Customer { get; set; } = null!;
    public ICollection<BillDetail>? BillDetails { get; set; } 
    private Bill(
        BillId billId,
        OrderId orderId,
        CustomerId customerId,
        DateTime createdDate,
        BillPaymentStatus billPaymentStatus,
        FullName fullName,
        PhoneNumber phoneNumber,
        Email email,
        SpecificAddress specificAddress,
        Province province,
        District district,
        Ward ward)
    {
        BillId = billId;
        OrderId = orderId;
        CustomerId = customerId;
        CreatedDate = createdDate;
        BillPaymentStatus = billPaymentStatus;
        FullName = fullName;
        PhoneNumber = phoneNumber;
        Email = email;
        SpecificAddress = specificAddress;
        Province = province;
        District = district;
        Ward = ward;
    }

    public static Bill NewBill(
        OrderId orderId,
        CustomerId customerId,
        DateTime createdDate,
        BillPaymentStatus billPaymentStatus,
        FullName fullName,
        PhoneNumber phoneNumber,
        Email email,
        SpecificAddress specificAddress,
        Province province,
        District district,
        Ward ward)
    {
        return new Bill(
            BillId.New(),
            orderId,
            customerId,
            createdDate,
            billPaymentStatus,
            fullName,
            phoneNumber,
            email,
            specificAddress,
            province,
            district,
            ward);
    }

    public void ChangeBillStatus(
        BillPaymentStatus billPaymentStatus, 
        PaymentMethod paymentMethod)
    {
        BillPaymentStatus = billPaymentStatus;
    }

    public void MarkAsRated()
    {
        IsRated = IsRated.Rated;
    }
}
