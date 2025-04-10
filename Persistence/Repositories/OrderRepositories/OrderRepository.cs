using Domain.Entities.Customers;
using Domain.Entities.OrderDetails;
using Domain.Entities.Orders;
using Domain.Entities.OrderTransactions;
using Domain.Entities.ProductVariants;
using Domain.IRepositories.Orders;
using MassTransit.Serialization;
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
        return await _postgreSQLWriteDbContext.Orders.FindAsync(orderId);
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
}
