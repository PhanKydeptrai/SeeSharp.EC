using Application.DTOs.Order;
using Application.IServices;
using Domain.Entities.Customers;
using Domain.Entities.Orders;
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
    
    public async Task<OrderResponse?> GetOrderById(OrderId orderId)
    {
        return await _dbContext.Orders
            .Where(a => a.OrderId == new Ulid(orderId) && a.OrderStatus != OrderStatus.Waiting)
            .Select(a => new OrderResponse(
                a.OrderId.ToGuid(),
                a.CustomerId.ToGuid(),
                a.Total,
                a.PaymentStatus.ToString(),
                a.OrderStatus.ToString(),
                a.OrderDetailReadModels.Select(b => new OrderDetailResponse(
                    b.OrderDetailId.ToGuid(),
                    b.ProductId.ToGuid(),
                    b.ProductReadModel.ProductPrice,
                    b.Quantity,
                    b.ProductReadModel.ImageUrl ?? string.Empty,
                    b.UnitPrice
                )).ToArray()
            )).FirstOrDefaultAsync();
    }

    public async Task<OrderResponse?> GetCartInformation(CustomerId customerId)
    {
        return await _dbContext.Orders
            .Where(a => a.CustomerId == new Ulid(customerId) && a.OrderStatus == OrderStatus.Waiting)
            .Select(a => new OrderResponse(
                a.OrderId.ToGuid(),
                a.CustomerId.ToGuid(),
                a.Total,
                a.PaymentStatus.ToString(),
                a.OrderStatus.ToString(),
                a.OrderDetailReadModels.Select(b => new OrderDetailResponse(
                    b.OrderDetailId.ToGuid(),
                    b.ProductId.ToGuid(),
                    b.ProductReadModel.ProductPrice,
                    b.Quantity,
                    b.ProductReadModel.ImageUrl ?? string.Empty,
                    b.UnitPrice
                )).ToArray()
            )).FirstOrDefaultAsync();
    }
}
