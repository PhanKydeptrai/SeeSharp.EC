using Domain.Entities.Orders;
using Domain.IRepositories.Orders;
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
    

}
