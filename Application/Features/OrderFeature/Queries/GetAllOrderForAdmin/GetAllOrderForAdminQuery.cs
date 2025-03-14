using Application.Abstractions.Messaging;
using Application.DTOs.Order;
using Application.Features.Pages;

namespace Application.Features.OrderFeature.Queries.GetAllOrderForAdmin;

public record GetAllOrderForAdminQuery(
    string? orderStatusFilter,
    string? customerFilter,
    string? searchTerm,
    string? sortColumn,
    string? sortOrder,
    int? page,
    int? pageSize) : IQuery<PagedList<OrderResponse>>;
