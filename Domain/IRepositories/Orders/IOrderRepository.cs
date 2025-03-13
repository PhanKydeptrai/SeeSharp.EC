using Domain.Entities.OrderDetails;
using Domain.Entities.Orders;
using Domain.Entities.OrderTransactions;
using Domain.Entities.Products;

namespace Domain.IRepositories.Orders;

public interface IOrderRepository
{
    Task AddNewOrderToMySQL(Order order);
    Task AddNewOrderToPostgreSQL(Order order);
    Task AddNewOrderDetailToMySQL(OrderDetail orderDetail);
    Task AddNewOrderDetailToPostgreSQL(OrderDetail orderDetail);
    Task AddNewOrderTransactionToMySQL(OrderTransaction orderTransaction);
    Task AddNewOrderTransactionToPostgreSQL(OrderTransaction orderTransaction);
    Task<OrderDetail?> CheckProductAvailabilityInOrder(OrderId orderId, ProductId productId);
    Task<OrderDetail?> GetOrderDetailByIdFromMySQL(OrderDetailId orderDetailId);
    Task<OrderDetail?> GetOrderDetailByIdFromPostgreSQL(OrderDetailId orderDetailId);
    Task<Order?> GetOrderByIdFromMySQL(OrderId orderId);
    Task<Order?> GetOrderByIdFromPostgreSQL(OrderId orderId);
}


