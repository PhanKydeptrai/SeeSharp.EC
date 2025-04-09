using System.Linq.Expressions;
using Application.DTOs.Order;
using Application.Features.Pages;
using Application.IServices;
using Domain.Database.PostgreSQL.ReadModels;
using Domain.Entities.Customers;
using Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Persistence.Database.PostgreSQL;

namespace Infrastructure.Services.OrderServices;

internal sealed class OrderQueryServices : IOrderQueryServices
{
    private readonly SeeSharpPostgreSQLReadDbContext _dbContext;

    public OrderQueryServices(SeeSharpPostgreSQLReadDbContext dbContext)
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
                    b.ProductVariantId.ToGuid(),
                    b.ProductVariantReadModel.ProductVariantPrice,
                    b.Quantity,
                    b.ProductVariantReadModel.ImageUrl ?? string.Empty,
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
                    b.ProductVariantId.ToGuid(),
                    b.ProductVariantReadModel.ProductVariantPrice,
                    b.Quantity,
                    b.ProductVariantReadModel.ImageUrl ?? string.Empty,
                    b.UnitPrice
                )).ToArray()
            )).FirstOrDefaultAsync();
    }

    public async Task<PagedList<OrderResponse>> GetAllOrderForAdmin(
            string? statusFilter,
            string? customerFilter,
            string? searchTerm,
            string? sortColumn,
            string? sortOrder,
            int? page,
            int? pageSize)
    {
        var orderQuery = _dbContext.Orders.Where(a => a.OrderStatus != OrderStatus.Waiting).AsQueryable();
        //Search
        // if (!string.IsNullOrEmpty(searchTerm))
        // {
        //     orderQuery = orderQuery.Where(x => x.CategoryName.Contains(searchTerm));
        // }

        //Filter
        if (!string.IsNullOrEmpty(statusFilter))
        {
            orderQuery = orderQuery
                .Where(x => x.OrderStatus == (OrderStatus)Enum.Parse(typeof(OrderStatus), statusFilter));
        }

        if (!string.IsNullOrEmpty(customerFilter) && Ulid.TryParse(customerFilter, out var _))
        {
            orderQuery = orderQuery
                .Where(x => x.CustomerId == Ulid.Parse(customerFilter));
        }

        //sort
        Expression<Func<OrderReadModel, object>> keySelector = sortColumn?.ToLower() switch
        {
            "orderid" => x => x.OrderId,
            _ => x => x.OrderId
        };

        if (sortOrder?.ToLower() == "desc")
        {
            orderQuery = orderQuery.OrderByDescending(keySelector);
        }
        else
        {
            orderQuery = orderQuery.OrderBy(keySelector);
        }

        //paged
        var orders = orderQuery
            .Select(a => new OrderResponse(
                a.OrderId.ToGuid(),
                a.CustomerId.ToGuid(),
                a.Total,
                a.PaymentStatus.ToString(),
                a.OrderStatus.ToString(),
                a.OrderDetailReadModels.Select(b => new OrderDetailResponse(
                    b.OrderDetailId.ToGuid(),
                    b.ProductVariantId.ToGuid(),
                    b.ProductVariantReadModel.ProductVariantPrice,
                    b.Quantity,
                    b.ProductVariantReadModel.ImageUrl ?? string.Empty,
                    b.UnitPrice
                )).ToArray())).AsQueryable();

        var orderList = await PagedList<OrderResponse>
            .CreateAsync(orders, page ?? 1, pageSize ?? 10);

        return orderList;
    }

    public async Task<PagedList<OrderResponse>> GetAllOrderForCustomer(
            CustomerId customerId,
            string? statusFilter,
            string? searchTerm,
            string? sortColumn,
            string? sortOrder,
            int? page,
            int? pageSize)
    {
        var orderQuery = _dbContext.Orders.Where(
            a => a.OrderStatus != OrderStatus.Waiting && a.CustomerId == customerId.ToUlid())
            .AsQueryable();
            
        //Search
        // if (!string.IsNullOrEmpty(searchTerm))
        // {
        //     orderQuery = orderQuery.Where(x => x.CategoryName.Contains(searchTerm));
        // }

        //Filter
        if (!string.IsNullOrEmpty(statusFilter))
        {
            orderQuery = orderQuery
                .Where(x => x.OrderStatus == (OrderStatus)Enum.Parse(typeof(OrderStatus), statusFilter));
        }

        

        //sort
        Expression<Func<OrderReadModel, object>> keySelector = sortColumn?.ToLower() switch
        {
            "orderid" => x => x.OrderId,
            _ => x => x.OrderId
        };

        if (sortOrder?.ToLower() == "desc")
        {
            orderQuery = orderQuery.OrderByDescending(keySelector);
        }
        else
        {
            orderQuery = orderQuery.OrderBy(keySelector);
        }

        //paged
        var orders = orderQuery
            .Select(a => new OrderResponse(
                a.OrderId.ToGuid(),
                a.CustomerId.ToGuid(),
                a.Total,
                a.PaymentStatus.ToString(),
                a.OrderStatus.ToString(),
                a.OrderDetailReadModels.Select(b => new OrderDetailResponse(
                    b.OrderDetailId.ToGuid(),
                    b.ProductVariantId.ToGuid(),
                    b.ProductVariantReadModel.ProductVariantPrice,
                    b.Quantity,
                    b.ProductVariantReadModel.ImageUrl ?? string.Empty,
                    b.UnitPrice
                )).ToArray())).AsQueryable();

        var orderList = await PagedList<OrderResponse>
            .CreateAsync(orders, page ?? 1, pageSize ?? 10);

        return orderList;
    }

    public async Task<MakePaymentResponse?> GetMakePaymentResponse(CustomerId customerId)
    {
        return await _dbContext.Orders
            .Include(a => a.OrderTransactionReadModel)
            .ThenInclude(a => a!.VoucherReadModel)
            .Include(a => a.OrderDetailReadModels)
            .ThenInclude(a => a.ProductVariantReadModel)
            .Where(a => a.CustomerId == new Ulid(customerId) && a.OrderStatus == OrderStatus.Waiting)
            .Select(a => new MakePaymentResponse(
                a.OrderId.ToGuid(),
                a.PaymentStatus.ToString(),
                a.OrderTransactionReadModel!.Amount,
                a.OrderTransactionReadModel.VoucherReadModel!.VoucherCode,
                a.OrderDetailReadModels.Select(b => new OrderDetailResponse(
                    b.OrderDetailId.ToGuid(),
                    b.ProductVariantId.ToGuid(),
                    b.ProductVariantReadModel.ProductVariantPrice,
                    b.Quantity,
                    b.ProductVariantReadModel.ImageUrl ?? string.Empty,
                    b.UnitPrice
                )).ToArray()
            )).FirstOrDefaultAsync();
    }
}
