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
    Task<Order?> GetWaitingOrderByCustomerId(CustomerId customerId);

    Task<OrderTransaction?> GetOrderTransactionById(OrderTransactionId orderTransactionId);

    /// <summary>
    /// lấy order transaction theo customerId
    /// </summary>
    /// <param name="customerId"></param>
    /// <returns></returns>
    Task<OrderTransaction?> GetOrderTransactionByCustomerId(CustomerId customerId);
    void RemoveOrderTransaction(OrderTransaction orderTransaction); 
    // Task SyncCartForCustomer(OrderId orderId, CustomerId customerId);
    // Task<Order?> GetOrderByGuestId(CustomerId customerId);
    void DeleteOrderDetail(OrderDetail orderDetail);
}


