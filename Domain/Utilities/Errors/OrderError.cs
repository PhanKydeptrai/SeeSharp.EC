using Domain.Entities.Bills;
using Domain.Entities.Customers;
using Domain.Entities.OrderDetails;
using Domain.Entities.Orders;
using Domain.Entities.OrderTransactions;
using SharedKernel;

namespace Domain.Utilities.Errors;

public static class OrderError
{
    public static Error OrderDetailNotFound(OrderDetailId orderDetailId) =>  Error.NotFound(
        "OrderDetatail.NotFound",
        $"The Order detail with the Id = '{orderDetailId}' was not found");
    
    public static Error OrderNotFound(OrderId orderId) => Error.NotFound(
        "Order.NotFound",
        $"The Order with the Id = '{orderId}' was not found");

    public static Error OrderNotExist() => Error.Problem(
        "Order.NotExist",
        $"The order does not exist for the given customer.");

    public static Error OrderNotCreated(CustomerId customerId) => Error.Problem(
        "Order.NotCreated",
        $"Customer with the Id = '{customerId}' dont have any order yet, Add product to create a new order"); 
    
    public static Error TransactionNotCreated(CustomerId customerId) => Error.NotFound(
        "OrderTransaction.NotCreated",
        $"Customer with the Id = '{customerId}' dont have any transaction yet");

    public static Error TransactionNotFound(OrderTransactionId orderTransactionId) => Error.NotFound(
        "OrderTransaction.NotCreated",
        $"Customer with the Id = '{orderTransactionId}' dont have any transaction yet");

    /// <summary>
    /// OrderStatus không hợp lệ
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    public static Error OrderStatusInValid(OrderId orderId) => Error.Problem(
        "OrderStatus.Invalid",
        $"The Order with the Id = '{orderId}' is not in a valid state to perform this action");

    /// <summary>
    /// BillStatus không hợp lệ
    /// </summary>
    /// <param name="billId"></param>
    /// <returns></returns>
    public static Error BillStatusInValid(BillId billId) => Error.Problem(
        "BillStatus.Invalid",
        $"The Order with the Id = '{billId}' is not in a valid state to perform this action");
}
