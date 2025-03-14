using Application.Abstractions.Messaging;
using Application.DTOs.Order;
using Application.Features.Pages;

namespace Application.Features.OrderFeature.Queries.GetAllOrderForCustomer;

public record GetAllOrderForCustomerQuery(
    Guid customerId,
    string? statusFilter,
    string? searchTerm,
    string? sortColumn,
    string? sortOrder,
    int? page,
    int? pageSize) : IQuery<PagedList<OrderResponse>>;
