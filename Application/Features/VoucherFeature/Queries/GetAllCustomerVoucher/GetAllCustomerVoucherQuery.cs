using Application.Abstractions.Messaging;
using Application.DTOs.Voucher;
using Application.Features.Pages;

namespace Application.Features.VoucherFeature.Queries.GetAllCustomerVoucher;

public record GetAllCustomerVoucherQuery(
    Guid CustomerId,
    string? voucherTypeFilter,
    string? statusFilter,
    string? searchTerm,
    string? sortColumn,
    string? sortOrder,
    int? page,
    int? pageSize) : IQuery<PagedList<CustomerVoucherResponse>>;
