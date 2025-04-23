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
    public CustomerType CustomerType { get; private set; }
    public User? User { get; set; } = null!;
    //Foreign key
    public ICollection<ShippingInformation>? ShippingInformations { get; set; } = null!;
    public ICollection<CustomerVoucher>? CustomerVouchers { get; set; } = null!;
    public ICollection<WishItem>? WishItems { get; set; } = null!;
    public ICollection<Order>? Orders { get; set; } = null!;
    public ICollection<Feedback>? Feedbacks { get; set; } = null!;
    public ICollection<Bill>? Bills { get; set; } = null!;

    private Customer(
        CustomerId customerId, 
        UserId userId, 
        CustomerType customerType)
    {
        CustomerId = customerId;
        UserId = userId;
        CustomerType = customerType;
    }

    //* factory method
    public static Customer NewCustomer(
        UserId userId,
        CustomerType customerType)
    {
        return new Customer(
            CustomerId.New(), 
            userId,
            customerType);
    }

    public static Customer FromExisting(
        CustomerId customerId,
        UserId userId,
        CustomerType customerType)
    {
        return new Customer(
            customerId, 
            userId, 
            customerType);
    }
}


