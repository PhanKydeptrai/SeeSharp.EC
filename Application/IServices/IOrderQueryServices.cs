using Application.DTOs.Bills;
using Application.DTOs.Order;
using Application.Features.Pages;
using Domain.Entities.Bills;
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
    /// 
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    Task<MakePaymentResponse?> GetMakePaymentResponse(CustomerId customerId);

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
    Task<List<OrderHistoryResponse>> GetOrderHistoryForCustomer(CustomerId customerId);

    /// <summary>
    /// Lấy thông tin hóa đơn theo BillId
    /// </summary>
    /// <param name="billId"></param>
    /// <returns></returns>
    Task<BillResponse?> GetBillByBillId(BillId billId);


    /// <summary>
    /// Lấy thông tin hoá đơn bằng OrderId
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    Task<BillResponse?> GetBillByOrderId(OrderId orderId);
}
