using System.Linq.Expressions;
using Application.DTOs.Bills;
using Application.DTOs.Order;
using Application.Features.Pages;
using Application.IServices;
using Domain.Database.PostgreSQL.ReadModels;
using Domain.Entities.Bills;
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
                    b.ProductVariantReadModel.ProductReadModel!.ProductName,
                    b.ProductVariantReadModel.VariantName,
                    b.ProductVariantReadModel.ColorCode,
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
                    b.ProductVariantReadModel.ProductReadModel!.ProductName,
                    b.ProductVariantReadModel.VariantName,
                    b.ProductVariantReadModel.ColorCode,
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
                    b.ProductVariantReadModel.ProductReadModel!.ProductName,
                    b.ProductVariantReadModel.VariantName,
                    b.ProductVariantReadModel.ColorCode,
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
                    b.ProductVariantReadModel.ProductReadModel!.ProductName,
                    b.ProductVariantReadModel.VariantName,
                    b.ProductVariantReadModel.ColorCode,
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
                    b.ProductVariantReadModel.ProductReadModel!.ProductName,
                    b.ProductVariantReadModel.VariantName,
                    b.ProductVariantReadModel.ColorCode,
                    b.ProductVariantReadModel.ProductVariantPrice,
                    b.Quantity,
                    b.ProductVariantReadModel.ImageUrl ?? string.Empty,
                    b.UnitPrice
                )).ToArray()
            )).FirstOrDefaultAsync();
    }


    //NOTE: Sửa lại sử dụng với readmodel //FIXME
    public async Task<List<OrderHistoryResponse>> GetOrderHistoryForCustomer(CustomerId customerId)
    {
        throw new NotImplementedException("This method is not implemented yet. Please use GetAllOrderForCustomer instead.");
        //return await _dbContext.Orders
        //    .Include(a => a.OrderTransactionReadModel)
        //    .ThenInclude(a => a!.VoucherReadModel)
        //    .Include(a => a.BillReadModel)
        //    .ThenInclude(a => a!.ShippingInformationReadModel)
        //    .Include(a => a.OrderDetailReadModels)
        //    .Include(a => a.CustomerReadModel)
        //    .ThenInclude(a => a.UserReadModel)
        //    .Where(a => a.CustomerId == new Ulid(customerId) && a.OrderStatus != OrderStatus.Waiting)
        //    .Select(a => new OrderHistoryResponse(
        //        a.CustomerId.ToGuid(),
        //        a.CustomerReadModel!.UserReadModel!.UserName,
        //        a.CustomerReadModel.UserReadModel.PhoneNumber!,
        //        a.BillReadModel!.ShippingInformationReadModel.SpecificAddress,
        //        a.Total,
        //        a.PaymentStatus.ToString(),
        //        a.OrderStatus.ToString(),
        //        a.BillReadModel!.PaymentMethod.ToString(),
        //        a.OrderTransactionReadModel!.IsVoucherUsed && a.OrderTransactionReadModel.VoucherReadModel != null ? a.OrderTransactionReadModel.VoucherReadModel.VoucherCode : null,
        //        a.BillReadModel.BillId.ToGuid(),
        //        a.OrderTransactionReadModel.Amount,
        //        a.OrderId.ToGuid(),
        //        a.OrderDetailReadModels!.Select(b => new OrderDetailResponse(
        //            b.OrderDetailId.ToGuid(),
        //            b.ProductVariantId.ToGuid(),
        //            b.ProductVariantReadModel!.ProductReadModel!.ProductName,
        //            b.ProductVariantReadModel.VariantName,
        //            b.ProductVariantReadModel.ColorCode,
        //            b.ProductVariantReadModel.ProductVariantPrice,
        //            b.Quantity,
        //            b.ProductVariantReadModel.ImageUrl ?? string.Empty,
        //            b.UnitPrice
        //        )).ToArray()
        //    ))
        //    .ToListAsync();
    }

    public async Task<BillResponse?> GetBillByBillId(BillId billId)
    {
        throw new NotImplementedException("This method is not implemented yet. Please use GetBillByOrderId instead.");
        //return await _dbContext.Orders
        //    .Include(a => a.OrderTransactionReadModel)
        //    .ThenInclude(a => a!.VoucherReadModel)
        //    .Include(a => a.BillReadModel)
        //    .ThenInclude(a => a!.ShippingInformationReadModel)
        //    .Include(a => a.OrderDetailReadModels)
        //    .Include(a => a.CustomerReadModel)
        //    .ThenInclude(a => a!.UserReadModel)
        //    .Where(a => a.BillReadModel!.BillId == new Ulid(billId) && a.OrderStatus != OrderStatus.Waiting)
        //    .Select(a => new BillResponse(
        //        a.CustomerId.ToGuid(),
        //        a.CustomerReadModel!.UserReadModel!.UserName,
        //        a.CustomerReadModel.UserReadModel.Email!,
        //        a.BillReadModel!.ShippingInformationReadModel.PhoneNumber!,
        //        a.BillReadModel!.ShippingInformationReadModel.SpecificAddress,
        //        a.Total,
        //        a.PaymentStatus.ToString(),
        //        a.OrderStatus.ToString(),
        //        a.BillReadModel!.PaymentMethod.ToString(),
        //        a.OrderTransactionReadModel!.IsVoucherUsed && a.OrderTransactionReadModel.VoucherReadModel != null ? a.OrderTransactionReadModel.VoucherReadModel.VoucherCode : null,
        //        a.BillReadModel.BillId.ToGuid(),
        //        a.OrderTransactionReadModel.Amount,
        //        a.OrderId.ToGuid(),
        //        a.OrderDetailReadModels!.Select(b => new OrderDetailResponse(
        //            b.OrderDetailId.ToGuid(),
        //            b.ProductVariantId.ToGuid(),
        //            b.ProductVariantReadModel!.ProductReadModel!.ProductName,
        //            b.ProductVariantReadModel.VariantName,
        //            b.ProductVariantReadModel.ColorCode,
        //            b.ProductVariantReadModel.ProductVariantPrice,
        //            b.Quantity,
        //            b.ProductVariantReadModel.ImageUrl ?? string.Empty,
        //            b.UnitPrice
        //        )).ToArray()
        //    )).FirstOrDefaultAsync();

        #region Old Code
        //return await _writeDbContext.Orders
        //    .Include(a => a.OrderTransaction)
        //    .ThenInclude(a => a!.Voucher)
        //    .Include(a => a.Bill)
        //    .ThenInclude(a => a!.ShippingInformation)
        //    .Include(a => a.OrderDetails)
        //    .Include(a => a.Customer)
        //    .ThenInclude(a => a!.User)
        //    .Where(a => a.Bill!.BillId == billId && a.OrderStatus != OrderStatus.Waiting)
        //    .Select(a => new BillResponse(
        //        a.CustomerId.Value,
        //        a.Customer!.User!.UserName.Value,
        //        a.Customer.User.Email!.Value,
        //        a.Bill!.ShippingInformation.PhoneNumber!.Value,
        //        a.Bill!.ShippingInformation.SpecificAddress.Value,
        //        a.Total.Value,
        //        a.PaymentStatus.ToString(),
        //        a.OrderStatus.ToString(),
        //        a.Bill!.PaymentMethod.ToString(),
        //        a.OrderTransaction!.IsVoucherUsed.Value && a.OrderTransaction.Voucher != null ? a.OrderTransaction.Voucher.VoucherCode.Value : null,
        //        a.Bill.BillId.Value,
        //        a.OrderTransaction.Amount.Value,
        //        a.OrderId.Value,
        //        a.OrderDetails!.Select(b => new OrderDetailResponse(
        //            b.OrderDetailId.Value,
        //            b.ProductVariantId.Value,
        //            b.ProductVariant!.Product!.ProductName.Value,
        //            b.ProductVariant.VariantName.Value,
        //            b.ProductVariant.ColorCode.Value,
        //            b.ProductVariant.ProductVariantPrice.Value,
        //            b.Quantity.Value,
        //            b.ProductVariant.ImageUrl ?? string.Empty,
        //            b.UnitPrice.Value
        //        )).ToArray()
        //    )).FirstOrDefaultAsync();
        #endregion
    }

    public async Task<BillResponse?> GetBillByOrderId(OrderId orderId)
    {
        //return await _dbContext.Orders
        //    .Include(a => a.OrderTransactionReadModel)
        //    .ThenInclude(a => a!.VoucherReadModel)
        //    .Include(a => a.BillReadModel)
        //    .ThenInclude(a => a!.ShippingInformationReadModel)
        //    .Include(a => a.OrderDetailReadModels)
        //    .Include(a => a.CustomerReadModel)
        //    .ThenInclude(a => a!.UserReadModel)
        //    .Where(a => a.BillReadModel!.OrderId == new Ulid(orderId) && a.OrderStatus != OrderStatus.Waiting)
        //    .Select(a => new BillResponse(
        //        a.CustomerId.ToGuid(),
        //        a.CustomerReadModel!.UserReadModel!.UserName != string.Empty ? a.CustomerReadModel.UserReadModel.UserName : a.BillReadModel!.ShippingInformationReadModel.FullName,
        //        a.CustomerReadModel.UserReadModel.Email,
        //        a.BillReadModel!.ShippingInformationReadModel.PhoneNumber,
        //        a.BillReadModel!.ShippingInformationReadModel.SpecificAddress,
        //        a.Total,
        //        a.PaymentStatus.ToString(),
        //        a.OrderStatus.ToString(),
        //        a.BillReadModel!.PaymentMethod.ToString(),
        //        a.OrderTransactionReadModel!.IsVoucherUsed && a.OrderTransactionReadModel.VoucherReadModel != null ? a.OrderTransactionReadModel.VoucherReadModel.VoucherCode : null,
        //        a.BillReadModel.BillId.ToGuid(),
        //        a.OrderTransactionReadModel.Amount,
        //        a.OrderId.ToGuid(),
        //        a.OrderDetailReadModels!.Select(b => new OrderDetailResponse(
        //            b.OrderDetailId.ToGuid(),
        //            b.ProductVariantId.ToGuid(),
        //            b.ProductVariantReadModel!.ProductReadModel!.ProductName,
        //            b.ProductVariantReadModel.VariantName,
        //            b.ProductVariantReadModel.ColorCode,
        //            b.ProductVariantReadModel.ProductVariantPrice,
        //            b.Quantity,
        //            b.ProductVariantReadModel.ImageUrl ?? string.Empty,
        //            b.UnitPrice
        //        )).ToArray()

        throw new NotImplementedException("This method is not implemented yet. Please use GetBillByBillId instead.");
        //    )).FirstOrDefaultAsync();
    }

    public async Task<bool> IsOrderStatusDelivered(OrderId orderId)
    {
        return await _dbContext.Orders.AnyAsync(
            a => a.OrderStatus == OrderStatus.Delivered 
            && a.OrderId == orderId.ToUlid());
    }

    
    public async Task<bool> IsOrderStatusDelivered(BillId billId)
    {
        return await _dbContext.Orders.AnyAsync(
            a => a.OrderStatus == OrderStatus.Delivered
            && a.BillReadModel.BillId == billId.ToUlid());
    }
}
