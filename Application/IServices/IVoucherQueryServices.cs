using Application.DTOs.Voucher;
using Application.Features.Pages;
using Domain.Entities.Customers;
using Domain.Entities.Vouchers;

namespace Application.IServices;

public interface IVoucherQueryServices
{
    /// <summary>
    /// Get voucher by id
    /// </summary>
    /// <param name="voucherId">Voucher identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Voucher response or null if not found</returns>
    Task<VoucherResponse?> GetVoucherById(VoucherId voucherId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get voucher by code
    /// </summary>
    /// <param name="voucherCode">Voucher code</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Voucher response or null if not found</returns>
    Task<VoucherResponse?> GetVoucherByCode(VoucherCode voucherCode, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Check if voucher code exists
    /// </summary>
    /// <param name="voucherCode">Voucher code to check</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the voucher code exists, otherwise false</returns>
    Task<bool> IsVoucherCodeExist(VoucherCode voucherCode, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Check if voucher name exists
    /// </summary>
    /// <param name="voucherName">Voucher name to check</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the voucher name exists, otherwise false</returns>
    Task<bool> IsVoucherNameExist(VoucherName voucherName, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get all vouchers with filtering, sorting and pagination
    /// </summary>
    /// <param name="voucherTypeFilter">Filter by voucher type</param>
    /// <param name="statusFilter">Filter by status</param>
    /// <param name="searchTerm">Search term for voucher name or code</param>
    /// <param name="sortColumn">Column to sort by</param>
    /// <param name="sortOrder">Sort order (asc or desc)</param>
    /// <param name="page">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paged list of voucher responses</returns>
    Task<PagedList<VoucherResponse>> GetAllVouchers(
        string? voucherTypeFilter,
        string? statusFilter,
        string? searchTerm,
        string? sortColumn,
        string? sortOrder,
        int? page,
        int? pageSize,
        CancellationToken cancellationToken = default);
        
    /// <summary>
    /// Get all vouchers for a specific customer with filtering, sorting and pagination
    /// </summary>
    /// <param name="customerId">Customer identifier</param>
    /// <param name="voucherTypeFilter">Filter by voucher type</param>
    /// <param name="statusFilter">Filter by status</param>
    /// <param name="searchTerm">Search term for voucher name or code</param>
    /// <param name="sortColumn">Column to sort by</param>
    /// <param name="sortOrder">Sort order (asc or desc)</param>
    /// <param name="page">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paged list of customer voucher responses</returns>
    Task<PagedList<CustomerVoucherResponse>> GetAllCustomerVouchers(
        CustomerId customerId,
        string? voucherTypeFilter,
        string? statusFilter,
        string? searchTerm,
        string? sortColumn,
        string? sortOrder,
        int? page,
        int? pageSize,
        CancellationToken cancellationToken = default);
} 