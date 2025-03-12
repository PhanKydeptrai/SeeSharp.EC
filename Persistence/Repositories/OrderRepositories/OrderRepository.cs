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
}
