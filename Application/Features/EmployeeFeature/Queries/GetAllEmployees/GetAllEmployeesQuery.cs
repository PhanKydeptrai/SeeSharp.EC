using Application.DTOs.Employee;
using Application.Features.Pages;
using Application.Abstractions.Messaging;

namespace Application.Features.EmployeeFeature.Queries.GetAllEmployees;

public record GetAllEmployeesQuery(
    string? StatusFilter,
    string? RoleFilter,
    string? SearchTerm,
    string? SortColumn,
    string? SortOrder,
    int? Page,
    int? PageSize) : IQuery<PagedList<EmployeeResponse>>; 