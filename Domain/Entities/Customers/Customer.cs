using Domain.Entities.Bills;
using Domain.Entities.CustomerVouchers;
using Domain.Entities.Feedbacks;
using Domain.Entities.Orders;
using Domain.Entities.ShippingInformations;
using Domain.Entities.Users;
using Domain.Entities.WishItems;

namespace Domain.Entities.Customers;
public sealed class Customer
{
    public CustomerId CustomerId { get; private set; } = null!;
    public UserId UserId { get; private set; } = null!;
    public CustomerStatus CustomerStatus { get; private set; }
    public CustomerType CustomerType { get; private set; }
    public User? User { get; private set; } = null!;
    //Foreign key
    public ICollection<ShippingInformation>? ShippingInformations { get; set; } = null!;
    public ICollection<CustomerVoucher>? CustomerVouchers { get; set; } = null!;
    public ICollection<WishItem>? WishItems { get; set; } = null!;
    public ICollection<Order>? Orders { get; set; } = null!;
    public ICollection<Feedback>? Feedbacks { get; set; } = null!;
    public ICollection<Bill>? Bills { get; set; } = null!;

    public Customer(
        CustomerId customerId, 
        UserId userId, 
        CustomerStatus customerStatus, 
        CustomerType customerType)
    {
        CustomerId = customerId;
        UserId = userId;
        CustomerStatus = customerStatus;
        CustomerType = customerType;
    }

    //* factory method
    public static Customer NewCustomer(
        UserId userId,
        CustomerType customerType)
    {
        return new Customer(
            CustomerId.New(), 
            userId, CustomerStatus.Active, 
            customerType);
    }

    public static Customer FromExisting(
        CustomerId customerId,
        UserId userId,
        CustomerStatus customerStatus,
        CustomerType customerType)
    {
        return new Customer(
            customerId, 
            userId, 
            customerStatus, 
            customerType);
    }
}


