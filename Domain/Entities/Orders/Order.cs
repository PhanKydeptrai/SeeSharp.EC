using Domain.Entities.Customers;
using Domain.Entities.OrderDetails;

namespace Domain.Entities.Orders;

public sealed class Order
{
    public OrderId OrderId { get; private set; } = null!;
    public CustomerId CustomerId { get; private set; } = null!;
    public OrderTotal Total { get; private set; } = null!;
    public OrderPaymentStatus PaymentStatus { get; private set; }
    public OrderStatus OrderStatus { get; private set; }
    public Ulid OrderTransactionId { get; private set; } //FIXME: Change to OrderTransactionId
    //* Foreign Key
    public ICollection<OrderDetail>? OrderDetails { get; set; } = null!;
    // public OrderTransaction OrderTransaction { get; set; } = null!; //FIXME: Change to OrderTransaction
    public Customer? Customer { get; set; } = null!;
    //public Feedback? Feedback { get; set; } = null!; //FIXME: Change to Feedback
    //public Bill? Bill { get; set; } = null!; //FIXME: Change to Bill


    private Order(
        OrderId orderId, 
        CustomerId customerId, 
        OrderTotal total, 
        OrderPaymentStatus paymentStatus, 
        OrderStatus orderStatus, 
        Ulid orderTransactionId)
    {
        OrderId = orderId;
        CustomerId = customerId;
        Total = total;
        PaymentStatus = paymentStatus;
        OrderStatus = orderStatus;
        OrderTransactionId = orderTransactionId;
    }

    public static Order NewOrder(
        OrderId orderId, 
        CustomerId customerId, 
        OrderTotal total, 
        OrderPaymentStatus paymentStatus, 
        OrderStatus orderStatus, 
        Ulid orderTransactionId)
    {
        return new Order(
            orderId, 
            customerId, 
            total, 
            paymentStatus, 
            orderStatus, 
            orderTransactionId);
    }
}

