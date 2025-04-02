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
    public async Task AddNewOrderToPostgreSQL(Order order)
    {
        await _postgreSQLWriteDbContext.Orders.AddAsync(order);
    }

    public async Task AddNewOrderDetailToMySQL(OrderDetail orderDetail)
    {
        await _postgreSQLWriteDbContext.OrderDetails.AddAsync(orderDetail);
    }

    public async Task AddNewOrderDetailToPostgreSQL(OrderDetail orderDetail)
    {
        await _postgreSQLWriteDbContext.OrderDetails.AddAsync(orderDetail);
    }

    public async Task AddNewOrderTransactionToPostgreSQL(OrderTransaction orderTransaction)
    {
        await _postgreSQLWriteDbContext.OrderTransactions.AddAsync(orderTransaction);
    }

    public async Task<OrderDetail?> CheckProductAvailabilityInOrder(OrderId orderId, ProductVariantId productVariantId)
    {
        return await _postgreSQLWriteDbContext.OrderDetails
            .FirstOrDefaultAsync(x => x.OrderId == orderId && x.ProductVariantId == productVariantId);
    }

    public async Task<OrderDetail?> GetOrderDetailByIdFromPostgreSQL(OrderDetailId orderDetailId)
    {
        return await _postgreSQLWriteDbContext.OrderDetails
            .Include(a => a.Order)
            .Where(a => a.OrderDetailId == orderDetailId)
            .FirstOrDefaultAsync();
    }


    public async Task<Order?> GetOrderByIdFromPostgreSQL(OrderId orderId)
    {
        return await _postgreSQLWriteDbContext.Orders.FindAsync(orderId);
    }

    public async Task<Order?> GetOrderByCustomerIdFromPostgreSQL(CustomerId customerId)
    {
        return await _postgreSQLWriteDbContext.Orders
            .FirstOrDefaultAsync(x => x.CustomerId == customerId);
    }

    public void DeleteOrderDetailFromPostgeSQL(OrderDetail orderDetail)
    {
        _postgreSQLWriteDbContext.OrderDetails.Remove(orderDetail);
    }

}
