using Domain.Entities.Customers;
using Domain.Entities.OrderDetails;
using Domain.Entities.Orders;
using Domain.Entities.OrderTransactions;
using Domain.Entities.Products;
using Domain.IRepositories.Orders;
using Microsoft.EntityFrameworkCore;
using Persistence.Database.MySQL;
using Persistence.Database.PostgreSQL;

namespace Persistence.Repositories.OrderRepositories;

internal sealed class OrderRepository : IOrderRepository
{
    private readonly NextSharpMySQLWriteDbContext _mySqlWriteDbContext;
    private readonly NextSharpPostgreSQLWriteDbContext _postgreSQLWriteDbContext;

    public OrderRepository(
        NextSharpMySQLWriteDbContext mySqlWriteDbContext,
        NextSharpPostgreSQLWriteDbContext postgreSQLWriteDbContext)
    {
        _mySqlWriteDbContext = mySqlWriteDbContext;
        _postgreSQLWriteDbContext = postgreSQLWriteDbContext;
    }

    public async Task AddNewOrderToMySQL(Order order)
    {
        await _mySqlWriteDbContext.Orders.AddAsync(order);
    }

    public async Task AddNewOrderToPostgreSQL(Order order)
    {
        await _postgreSQLWriteDbContext.Orders.AddAsync(order);
    }

    public async Task AddNewOrderDetailToMySQL(OrderDetail orderDetail)
    {
        await _mySqlWriteDbContext.OrderDetails.AddAsync(orderDetail);
    }

    public async Task AddNewOrderDetailToPostgreSQL(OrderDetail orderDetail)
    {
        await _postgreSQLWriteDbContext.OrderDetails.AddAsync(orderDetail);
    }
    public async Task AddNewOrderTransactionToMySQL(OrderTransaction orderTransaction)
    {
        await _mySqlWriteDbContext.OrderTransactions.AddAsync(orderTransaction);
    }

    public async Task AddNewOrderTransactionToPostgreSQL(OrderTransaction orderTransaction)
    {
        await _postgreSQLWriteDbContext.OrderTransactions.AddAsync(orderTransaction);
    }

    public async Task<OrderDetail?> CheckProductAvailabilityInOrder(OrderId orderId, ProductId productId)
    {
        return await _mySqlWriteDbContext.OrderDetails
            .FirstOrDefaultAsync(x => x.OrderId == orderId && x.ProductId == productId);
    }
    /// <summary>
    /// Get order detail by OrderDetailId
    /// </summary>
    /// <param name="orderDetailId"></param>
    /// <returns></returns>
    public async Task<OrderDetail?> GetOrderDetailByIdFromMySQL(OrderDetailId orderDetailId)
    {
        // return await _mySqlWriteDbContext.OrderDetails.FindAsync(orderDetailId);
        return await _mySqlWriteDbContext.OrderDetails
            .Include(a => a.Order)
            .Where(a => a.OrderDetailId == orderDetailId).FirstOrDefaultAsync();
    }
    /// <summary>
    /// Get order detail by OrderDetailId
    /// </summary>
    /// <param name="orderDetailId"></param>
    /// <returns></returns>
    public async Task<OrderDetail?> GetOrderDetailByIdFromPostgreSQL(OrderDetailId orderDetailId)
    {
        return await _postgreSQLWriteDbContext.OrderDetails.FindAsync(orderDetailId);
    }
    /// <summary>
    /// Get order by OrderId
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    public async Task<Order?> GetOrderByIdFromMySQL(OrderId orderId)
    {
        return await _mySqlWriteDbContext.Orders.FindAsync(orderId);
    }
    /// <summary>
    /// Get order by OrderId
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    public async Task<Order?> GetOrderByIdFromPostgreSQL(OrderId orderId)
    {
        return await _postgreSQLWriteDbContext.Orders.FindAsync(orderId);
    }
    /// <summary>
    /// Get order by customer id
    /// </summary>
    /// <param name="customerId"></param>
    /// <returns></returns>
    public async Task<Order?> GetOrderByCustomerIdFromMySQL(CustomerId customerId)
    {
        return await _mySqlWriteDbContext.Orders
            .FirstOrDefaultAsync(x => x.CustomerId == customerId);
    }

}
