using Application.DTOs.Order;
using Domain.Entities.Customers;
using Domain.Entities.Orders;

namespace Application.IServices;

public interface IOrderQueryServices
{
    Task<OrderId?> CheckOrderAvailability(CustomerId customerId);
    /// <summary>
    /// Get Order and Order Details by OrderId with OrderStatus != New
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    Task<OrderResponse?> GetOrderById(OrderId orderId);
    
    /// <summary>
    /// Get Cart Information by CustomerId with OrderStatus == New
    /// </summary>
    /// <param name="customerId"></param>
    /// <returns></returns>
    Task<OrderResponse?> GetCartInformation(CustomerId customerId);
}
