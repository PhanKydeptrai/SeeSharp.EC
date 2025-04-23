using Application.Abstractions.Messaging;
using Application.DTOs.Customer;
using Application.Features.Pages;

namespace Application.Features.CustomerFeature.Queries.GetAllCustomer;

public record GetAllCustomerQuery(
    string? statusFilter,
    string? customerTypeFilter,
    string? searchTerm,
    string? sortColumn,
    string? sortOrder,
    int? page,
    int? pageSize) : IQuery<PagedList<CustomerProfileResponse>>;
