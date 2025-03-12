using System;
using Application.IServices;
using Domain.Entities.Customers;
using Domain.Entities.Orders;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Persistence.Database.PostgreSQL;

namespace Infrastructure.Services.OrderServices;

internal sealed class OrderQueryServices : IOrderQueryServices
{
    private readonly NextSharpPostgreSQLReadDbContext _dbContext;

    public OrderQueryServices(NextSharpPostgreSQLReadDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<OrderId?> CheckOrderAvailability(CustomerId customerId)
    {
        return await _dbContext.Orders
            .Where(x => x.CustomerId == new Ulid(customerId) && x.OrderStatus == OrderStatus.Waiting)
            .Select(x => OrderId.FromUlid(x.OrderId))
            .FirstOrDefaultAsync();
    }
}
