using Domain.Entities.Customers;
using Domain.Entities.OrderDetails;
using Domain.Entities.Orders;
using Domain.Entities.OrderTransactions;
using Domain.Entities.ProductVariants;
using Domain.IRepositories.Orders;
using Microsoft.EntityFrameworkCore;
using Persistence.Database.PostgreSQL;

namespace Persistence.Repositories.OrderRepositories;

internal sealed class OrderRepository : IOrderRepository
{
    private readonly SeeSharpPostgreSQLWriteDbContext _postgreSQLWriteDbContext;

    public OrderRepository(
        SeeSharpPostgreSQLWriteDbContext postgreSQLWriteDbContext)
    {
        _postgreSQLWriteDbContext = postgreSQLWriteDbContext;
    }
    public async Task AddNewOrder(Order order)
    {
        await _postgreSQLWriteDbContext.Orders.AddAsync(order);
    }

    public async Task AddNewOrderDetailToMySQL(OrderDetail orderDetail)
    {
        await _postgreSQLWriteDbContext.OrderDetails.AddAsync(orderDetail);
    }

    public async Task AddNewOrderDetail(OrderDetail orderDetail)
    {
        await _postgreSQLWriteDbContext.OrderDetails.AddAsync(orderDetail);
    }

    public async Task AddNewOrderTransaction(OrderTransaction orderTransaction)
    {
        await _postgreSQLWriteDbContext.OrderTransactions.AddAsync(orderTransaction);
    }

    public async Task<OrderDetail?> CheckProductAvailabilityInOrder(OrderId orderId, ProductVariantId productVariantId)
    {
        return await _postgreSQLWriteDbContext.OrderDetails
            .FirstOrDefaultAsync(x => x.OrderId == orderId && x.ProductVariantId == productVariantId);
    }

    public async Task<OrderDetail?> GetOrderDetailById(OrderDetailId orderDetailId)
    {
        return await _postgreSQLWriteDbContext.OrderDetails
            .Include(a => a.Order)
            .Where(a => a.OrderDetailId == orderDetailId)
            .FirstOrDefaultAsync();
    }


    public async Task<Order?> GetOrderById(OrderId orderId)
    {
        return await _postgreSQLWriteDbContext.Orders
            .Include(a => a.OrderTransaction)
            .Include(a => a.Bill)
            .FirstOrDefaultAsync(a => a.OrderId == orderId);
    }

    public async Task<Order?> GetOrderByCustomerId(CustomerId customerId)
    {
        return await _postgreSQLWriteDbContext.Orders
            .FirstOrDefaultAsync(x => x.CustomerId == customerId);
    }

    public async Task<Order?> GetOrderByGuestId(CustomerId customerId)
    {
        return await _postgreSQLWriteDbContext.Orders
            .Include(a => a.OrderDetails)
            .FirstOrDefaultAsync(x => x.CustomerId == customerId);
    }

    public async Task<Order?> GetWaitingOrderByCustomerId(CustomerId customerId)
    {
        return await _postgreSQLWriteDbContext.Orders
            .Include(a => a.OrderDetails)
            .FirstOrDefaultAsync(x => x.CustomerId == customerId && x.OrderStatus == OrderStatus.Waiting);
    }

    public void DeleteOrderDetail(OrderDetail orderDetail)
    {
        _postgreSQLWriteDbContext.OrderDetails.Remove(orderDetail);
    }

    public void RemoveOrderTransaction(OrderTransaction orderTransaction)
    {
        _postgreSQLWriteDbContext.OrderTransactions.Remove(orderTransaction);
    }

    public async Task<OrderTransaction?> GetOrderTransactionByCustomerId(CustomerId customerId)
    {
        return await _postgreSQLWriteDbContext.OrderTransactions
            .Include(a => a.Bill)
            .FirstOrDefaultAsync(
                x => x.Bill!.CustomerId == customerId
                && x.TransactionStatus == TransactionStatus.Pending);
    }

    public async Task<OrderTransaction?> GetOrderTransactionById(OrderTransactionId orderTransactionId)
    {
        return await _postgreSQLWriteDbContext.OrderTransactions
            .Include(a => a.Bill)
            .Include(a => a.Order)
            .FirstOrDefaultAsync(x => x.OrderTransactionId == orderTransactionId);
    }

    public async Task CancelOrder(Order order)
    {
        await _postgreSQLWriteDbContext.Orders
            .Where(x => x.OrderId == order.OrderId)
            .ExecuteUpdateAsync(x => x.SetProperty(a => a.OrderStatus, OrderStatus.Cancelled));
    }

    public async Task MergeOrder(Order guestOrder, Order orderCustomer)
    {
        // foreach(var item in guestOrder.OrderDetails!)
        // {
        //     foreach(var itemCustomer in orderCustomer.OrderDetails!)
        //     {
        //         if(item.ProductVariantId == itemCustomer.ProductVariantId)
        //         {
        //             itemCustomer.Quantity += item.Quantity;
        //             _postgreSQLWriteDbContext.OrderDetails.Remove(item);
        //             break;
        //         }
        //     }
        // }

        // var orderDetailOfCustomer = orderCustomer.OrderDetails!.ToList();

        // await _postgreSQLWriteDbContext.OrderDetails
        //     .Where(a =>
        //         a.OrderId == guestOrder.OrderId // Lấy OrderDetail của guest
        //         && !orderDetailOfCustomer.Any(c => c.ProductVariantId == a.ProductVariantId) 
        //         // Loại trừ các ProductVariantId đã tồn tại trong customer order
        //     )
        //     .ExecuteUpdateAsync(x => x.SetProperty(a => a.OrderId, orderCustomer.OrderId));

        await _postgreSQLWriteDbContext.OrderDetails.Where(
            a => a.OrderId == guestOrder.OrderId)
            .ExecuteUpdateAsync(x => x.SetProperty(a => a.OrderId, orderCustomer.OrderId));
    }
}
