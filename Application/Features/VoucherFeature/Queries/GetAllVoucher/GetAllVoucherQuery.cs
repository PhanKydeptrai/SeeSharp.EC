using Application.Abstractions.Messaging;
using Application.DTOs.Voucher;
using Application.Features.Pages;

namespace Application.Features.VoucherFeature.Queries.GetAllVoucher;

public record GetAllVoucherQuery(
    string? voucherTypeFilter,
    string? statusFilter,
    string? searchTerm,
    string? sortColumn,
    string? sortOrder,
    int? page,
    int? pageSize) : IQuery<PagedList<VoucherResponse>>;
