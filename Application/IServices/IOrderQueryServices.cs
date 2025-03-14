using Application.DTOs.Order;
using Application.Features.Pages;
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
    /// Get Cart Information by CustomerId with OrderStatus == Waiting
    /// </summary>
    /// <param name="customerId"></param>
    /// <returns></returns>
    Task<OrderResponse?> GetCartInformation(CustomerId customerId);

    /// <summary>
    /// Get all orders for the admin | Just get the orders with orderdstatus != 
    /// </summary>
    /// <param name="statusFilter"></param>
    /// <param name="customerFilter"></param>
    /// <param name="searchTerm"></param>
    /// <param name="sortColumn"></param>
    /// <param name="sortOrder"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<PagedList<OrderResponse>> GetAllOrderForAdmin(
            string? statusFilter,
            string? customerFilter,
            string? searchTerm,
            string? sortColumn,
            string? sortOrder,
            int? page,
            int? pageSize);

    Task<PagedList<OrderResponse>> GetAllOrderForCustomer(
        CustomerId customerId,
        string? statusFilter,
        string? searchTerm,
        string? sortColumn,
        string? sortOrder,
        int? page,
        int? pageSize);
}
