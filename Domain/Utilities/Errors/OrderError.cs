using Domain.Entities.Customers;
using Domain.Entities.OrderDetails;
using Domain.Entities.Orders;
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

    public static Error OrderNotCreated(CustomerId customerId) => Error.Problem(
        "Order.NotCreated",
        $"Customer with the Id = '{customerId}' dont have any order yet, Add product to create a new order"); 
}
