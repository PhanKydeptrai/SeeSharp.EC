using Domain.Entities.Customers;
using Domain.Entities.OrderDetails;
using Domain.Entities.Orders;
using Domain.Entities.OrderTransactions;
using Domain.Entities.Products;
using Domain.Entities.ProductVariants;

namespace Domain.IRepositories.Orders;

public interface IOrderRepository
{
    Task AddNewOrderToPostgreSQL(Order order);
    Task AddNewOrderDetailToPostgreSQL(OrderDetail orderDetail);
    Task AddNewOrderTransactionToPostgreSQL(OrderTransaction orderTransaction);
    Task<OrderDetail?> CheckProductAvailabilityInOrder(OrderId orderId, ProductVariantId ProductVariantId);
    Task<OrderDetail?> GetOrderDetailByIdFromPostgreSQL(OrderDetailId orderDetailId);
    Task<Order?> GetOrderByIdFromPostgreSQL(OrderId orderId);
    Task<Order?> GetOrderByCustomerIdFromPostgreSQL(CustomerId customerId);
    void DeleteOrderDetailFromPostgeSQL(OrderDetail orderDetail);
}


