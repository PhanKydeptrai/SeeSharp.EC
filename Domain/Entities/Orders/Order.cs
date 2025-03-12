using Domain.Entities.Bills;
using Domain.Entities.Customers;
using Domain.Entities.Feedbacks;
using Domain.Entities.OrderDetails;
using Domain.Entities.OrderTransactions;

namespace Domain.Entities.Orders;

public sealed class Order
{
    public OrderId OrderId { get; private set; } = null!;
    public CustomerId CustomerId { get; private set; } = null!;
    public OrderTotal Total { get; private set; } = null!;
    public OrderPaymentStatus PaymentStatus { get; private set; }
    public OrderStatus OrderStatus { get; private set; }
    //* Foreign Key
    public ICollection<OrderDetail>? OrderDetails { get; set; } = null!;
    public OrderTransaction OrderTransaction { get; set; } = null!; 
    public Customer? Customer { get; set; } = null!;
    public Feedback? Feedback { get; set; } = null!; 
    public Bill? Bill { get; set; } = null!;


    private Order(
        OrderId orderId, 
        CustomerId customerId, 
        OrderTotal total, 
        OrderPaymentStatus paymentStatus, 
        OrderStatus orderStatus)
    {
        OrderId = orderId;
        CustomerId = customerId;
        Total = total;
        PaymentStatus = paymentStatus;
        OrderStatus = orderStatus;
    }

    public static Order NewOrder(
        CustomerId customerId, 
        OrderTotal total, 
        OrderPaymentStatus paymentStatus, 
        OrderStatus orderStatus)
    {
        
        return new Order(
            OrderId.New(), 
            customerId, 
            total, 
            paymentStatus, 
            orderStatus);
    }
}

