using Domain.Entities.Customers;
using Domain.Entities.OrderDetails;
using Domain.Entities.Orders;
using Domain.Entities.OrderTransactions;
using Domain.Entities.ProductVariants;

namespace Domain.IRepositories.Orders;

public interface IOrderRepository
{
    /// <summary>
    /// Thêm mới order
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    Task AddNewOrder(Order order);

    /// <summary>
    /// Thêm mới order detail
    /// </summary>
    /// <param name="orderDetail"></param>
    /// <returns></returns>
    Task AddNewOrderDetail(OrderDetail orderDetail);

    /// <summary>
    /// Thêm mới order transaction
    /// </summary>
    /// <param name="orderTransaction"></param>
    /// <returns></returns>
    Task AddNewOrderTransaction(OrderTransaction orderTransaction);

    /// <summary>
    /// Kiểm tra xem sản phẩm đã có trong order chưa
    /// </summary>
    /// <param name="orderId"></param>
    /// <param name="ProductVariantId"></param>
    /// <returns></returns>
    Task<OrderDetail?> CheckProductAvailabilityInOrder(OrderId orderId, ProductVariantId ProductVariantId);

    /// <summary>
    /// Lấy danh sách order detail theo orderId
    /// </summary>
    /// <param name="orderDetailId"></param>
    /// <returns></returns>
    Task<OrderDetail?> GetOrderDetailById(OrderDetailId orderDetailId);
    
    /// <summary>
    /// Huỷ đơn hàng
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    Task CancelOrder(Order order);
    /// <summary>
    /// Lấy danh sách order detail theo orderId
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    Task<Order?> GetOrderById(OrderId orderId);

    /// <summary>
    /// Lấy danh sách order detail theo orderId
    /// </summary>
    /// <param name="customerId"></param>
    /// <returns></returns>
    Task<Order?> GetOrderByCustomerId(CustomerId customerId);
    Task<Order?> GetWaitingOrderByCustomerId(CustomerId customerId);

    /// <summary>
    /// lấy order transaction theo id
    /// </summary>
    /// <param name="orderTransactionId">Id của order transaction</param>
    /// <returns></returns>
    Task<OrderTransaction?> GetOrderTransactionById(OrderTransactionId orderTransactionId);

    /// <summary>
    /// lấy order transaction theo customerId
    /// </summary>
    /// <param name="customerId"></param>
    /// <returns></returns>
    Task<OrderTransaction?> GetOrderTransactionByCustomerId(CustomerId customerId);
    /// <summary>
    /// Xoá order transaction
    /// </summary>
    /// <param name="orderTransaction"></param>
    void RemoveOrderTransaction(OrderTransaction orderTransaction); 
    // Task SyncCartForCustomer(OrderId orderId, CustomerId customerId);
    // Task<Order?> GetOrderByGuestId(CustomerId customerId);
    void DeleteOrderDetail(OrderDetail orderDetail);
}


