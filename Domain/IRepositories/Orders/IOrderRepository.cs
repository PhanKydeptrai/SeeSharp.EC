using Domain.Entities.Customers;
using Domain.Entities.OrderDetails;
using Domain.Entities.Orders;
using Domain.Entities.OrderTransactions;
using Domain.Entities.ProductVariants;

namespace Domain.IRepositories.Orders;

public interface IOrderRepository
{
    Task AddNewOrder(Order order);
    Task AddNewOrderDetail(OrderDetail orderDetail);
    Task AddNewOrderTransaction(OrderTransaction orderTransaction);
    Task<OrderDetail?> CheckProductAvailabilityInOrder(OrderId orderId, ProductVariantId ProductVariantId);
    Task<OrderDetail?> GetOrderDetailById(OrderDetailId orderDetailId);
    Task<Order?> GetOrderById(OrderId orderId);
    Task<Order?> GetOrderByCustomerId(CustomerId customerId);
    void DeleteOrderDetail(OrderDetail orderDetail);
}


