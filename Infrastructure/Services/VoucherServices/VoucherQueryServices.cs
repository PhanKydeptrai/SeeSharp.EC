using Application.DTOs.Voucher;
using Application.Features.Pages;
using Application.IServices;
using Domain.Entities.Customers;
using Domain.Entities.Vouchers;
using Microsoft.EntityFrameworkCore;
using Persistence.Database.PostgreSQL;

namespace Infrastructure.Services.VoucherServices;

internal sealed class VoucherQueryServices : IVoucherQueryServices
{
    private readonly SeeSharpPostgreSQLReadDbContext _dbContext;

    public VoucherQueryServices(SeeSharpPostgreSQLReadDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<VoucherResponse?> GetVoucherById(
        VoucherId voucherId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Vouchers
            .Where(v => v.VoucherId == new Ulid(voucherId.Value))
            .Select(v => new VoucherResponse(
                v.VoucherId.ToGuid(),
                v.VoucherName,
                v.VoucherCode,
                v.VoucherType.ToString(),
                v.PercentageDiscount.ToString(),
                v.MaximumDiscountAmount.ToString(),
                v.MinimumOrderAmount.ToString(),
                v.StartDate,
                v.ExpiredDate,
                v.Description,
                v.Status.ToString()))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<VoucherResponse?> GetVoucherByCode(
        VoucherCode voucherCode, 
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Vouchers
            .Where(v => v.VoucherCode == voucherCode.Value)
            .Select(v => new VoucherResponse(
                v.VoucherId.ToGuid(),
                v.VoucherName,
                v.VoucherCode,
                v.VoucherType.ToString(),
                v.PercentageDiscount.ToString(),
                v.MaximumDiscountAmount.ToString(),
                v.MinimumOrderAmount.ToString(),
                v.StartDate,
                v.ExpiredDate,
                v.Description,
                v.Status.ToString()))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> IsVoucherCodeExist(
        VoucherCode voucherCode, 
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Vouchers
            .AnyAsync(v => v.VoucherCode == voucherCode.Value, cancellationToken);
    }

    public async Task<bool> IsVoucherNameExist(
        VoucherName voucherName, 
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Vouchers
            .AnyAsync(v => v.VoucherName == voucherName.Value, cancellationToken);
    }
    
    public async Task<PagedList<VoucherResponse>> GetAllVouchers(
        string? voucherTypeFilter,
        string? statusFilter,
        string? searchTerm,
        string? sortColumn,
        string? sortOrder,
        int? page,
        int? pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Vouchers.AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(voucherTypeFilter))
        {
            if (Enum.TryParse<VoucherType>(voucherTypeFilter, true, out var voucherType))
            {
                query = query.Where(v => v.VoucherType == voucherType);
            }
        }

        if (!string.IsNullOrWhiteSpace(statusFilter))
        {
            if (Enum.TryParse<Status>(statusFilter, true, out var status))
            {
                query = query.Where(v => v.Status == status);
            }
        }

        // Apply search
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var searchTermLower = searchTerm.ToLower();
            query = query.Where(v => v.VoucherName.ToLower().Contains(searchTermLower) || 
                                    v.VoucherCode.ToLower().Contains(searchTermLower) ||
                                    v.Description.ToLower().Contains(searchTermLower));
        }

        // Apply sorting
        var isAscending = string.IsNullOrWhiteSpace(sortOrder) || sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase);
        
        query = (sortColumn?.ToLower()) switch
        {
            "vouchername" => isAscending 
                ? query.OrderBy(v => v.VoucherName) 
                : query.OrderByDescending(v => v.VoucherName),
                        
            "startdate" => isAscending 
                ? query.OrderBy(v => v.StartDate) 
                : query.OrderByDescending(v => v.StartDate),
            
            "expireddate" => isAscending 
                ? query.OrderBy(v => v.ExpiredDate) 
                : query.OrderByDescending(v => v.ExpiredDate),
            
            "status" => isAscending 
                ? query.OrderBy(v => v.Status) 
                : query.OrderByDescending(v => v.Status),
            
            // Default sorting
            _ => query.OrderByDescending(v => v.StartDate)
        };

        // Apply projection to DTO before pagination
        var projectedQuery = query.Select(v => new VoucherResponse(
            v.VoucherId.ToGuid(),
            v.VoucherName,
            v.VoucherCode,
            v.VoucherType.ToString(),
            v.PercentageDiscount.ToString(),
            v.MaximumDiscountAmount.ToString(),
            v.MinimumOrderAmount.ToString(),
            v.StartDate,
            v.ExpiredDate,
            v.Description,
            v.Status.ToString()));

        // Apply pagination
        return await PagedList<VoucherResponse>.CreateAsync(
            projectedQuery,
            page ?? 1,
            pageSize ?? 10);
    }
    
    public async Task<PagedList<CustomerVoucherResponse>> GetAllCustomerVouchers(
        CustomerId customerId,
        string? voucherTypeFilter,
        string? statusFilter,
        string? searchTerm,
        string? sortColumn,
        string? sortOrder,
        int? page,
        int? pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.CustomerVouchers
            .Where(cv => cv.CustomerId == new Ulid(customerId.Value))
            .Join(_dbContext.Vouchers,
                cv => cv.VoucherId,
                v => v.VoucherId,
                (cv, v) => new { CustomerVoucher = cv, Voucher = v });

        // Apply search
        if (!string.IsNullOrEmpty(searchTerm))
        {
            var searchTermLower = searchTerm.ToLower();
            query = query.Where(x => 
                x.Voucher.VoucherName.ToLower().Contains(searchTermLower) || 
                x.Voucher.VoucherCode.ToLower().Contains(searchTermLower) ||
                x.Voucher.Description.ToLower().Contains(searchTermLower));
        }

        // Apply filters
        if (!string.IsNullOrWhiteSpace(voucherTypeFilter))
        {
            query = query.Where(x => x.Voucher.VoucherType == (VoucherType)Enum.Parse(typeof(VoucherType), voucherTypeFilter));
        }

        if (!string.IsNullOrWhiteSpace(statusFilter))
        {
            query = query.Where(x => x.Voucher.Status == (Status)Enum.Parse(typeof(Status), statusFilter));
        }

        // Apply sorting
        if (!string.IsNullOrWhiteSpace(sortColumn))
        {
            var columnLower = sortColumn.ToLower();
            var isDescending = sortOrder?.ToLower() == "desc";

            query = columnLower switch
            {
                "vouchername" => isDescending
                    ? query.OrderByDescending(x => x.Voucher.VoucherName)
                    : query.OrderBy(x => x.Voucher.VoucherName),
                "startdate" => isDescending
                    ? query.OrderByDescending(x => x.Voucher.StartDate)
                    : query.OrderBy(x => x.Voucher.StartDate),
                "expireddate" => isDescending
                    ? query.OrderByDescending(x => x.Voucher.ExpiredDate)
                    : query.OrderBy(x => x.Voucher.ExpiredDate),
                "status" => isDescending
                    ? query.OrderByDescending(x => x.Voucher.Status)
                    : query.OrderBy(x => x.Voucher.Status),
                "quantity" => isDescending
                    ? query.OrderByDescending(x => x.CustomerVoucher.Quantity)
                    : query.OrderBy(x => x.CustomerVoucher.Quantity),
                _ => isDescending
                    ? query.OrderByDescending(x => x.Voucher.StartDate)
                    : query.OrderBy(x => x.Voucher.StartDate)
            };
        }
        else
        {
            // Default sorting
            query = query.OrderByDescending(x => x.Voucher.StartDate);
        }

        // Apply projection to DTO before pagination
        var projectedQuery = query.Select(x => new CustomerVoucherResponse(
            x.CustomerVoucher.CustomerVoucherId.ToGuid(),
            x.CustomerVoucher.Quantity,
            x.Voucher.VoucherId.ToGuid(),
            x.Voucher.VoucherName,
            x.Voucher.VoucherCode,
            x.Voucher.VoucherType.ToString(),
            x.Voucher.PercentageDiscount.ToString(),
            x.Voucher.MaximumDiscountAmount.ToString(),
            x.Voucher.MinimumOrderAmount.ToString(),
            x.Voucher.StartDate,
            x.Voucher.ExpiredDate,
            x.Voucher.Description,
            x.Voucher.Status.ToString()));

        // Apply pagination
        return await PagedList<CustomerVoucherResponse>.CreateAsync(
            projectedQuery,
            page ?? 1,
            pageSize ?? 10);
    }
}