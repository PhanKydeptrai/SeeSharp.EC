using Application.Abstractions.LinkService;
using Application.Abstractions.Messaging;
using Application.DTOs.Employee;
using Application.Features.Pages;
using Application.IServices;
using Domain.Entities.Users;
using SharedKernel;
using SharedKernel.Constants;

namespace Application.Features.EmployeeFeature.Queries.GetAllEmployees;

internal sealed class GetAllEmployeesQueryHandler : IQueryHandler<GetAllEmployeesQuery, PagedList<EmployeeResponse>>
{
    private readonly IEmployeeQueryServices _employeeQueryServices;
    private readonly ILinkServices _linkServices;
    
    public GetAllEmployeesQueryHandler(
        IEmployeeQueryServices employeeQueryServices,
        ILinkServices linkServices)
    {
        _employeeQueryServices = employeeQueryServices;
        _linkServices = linkServices;
    }

    public async Task<Result<PagedList<EmployeeResponse>>> Handle(
        GetAllEmployeesQuery request,
        CancellationToken cancellationToken)
    {
        var pagedList = await _employeeQueryServices.GetAllEmployees(
            request.StatusFilter,
            request.RoleFilter,
            request.SearchTerm,
            request.SortColumn,
            request.SortOrder,
            request.Page ?? 1,
            request.PageSize ?? 10);

        // AddLinks(request, pagedList);
        return Result.Success(pagedList);
    }

    // private void AddLinks(GetAllEmployeesQuery request, PagedList<EmployeeResponse> pagedList)
    // {
    //     pagedList.Links.Add(_linkServices.Generate(
    //         EndpointName.Employee.GetAll,
    //         new
    //         {
    //             statusFilter = request.StatusFilter,
    //             roleFilter = request.RoleFilter,
    //             searchTerm = request.SearchTerm,
    //             sortColumn = request.SortColumn,
    //             sortOrder = request.SortOrder,
    //             page = request.Page,
    //             pageSize = request.PageSize
    //         },
    //         "self",
    //         EndpointMethod.GET));

    //     if (pagedList.HaspreviousPage)
    //     {
    //         pagedList.Links.Add(_linkServices.Generate(
    //             EndpointName.Employee.GetAll,
    //             new
    //             {
    //                 statusFilter = request.StatusFilter,
    //                 roleFilter = request.RoleFilter,
    //                 searchTerm = request.SearchTerm,
    //                 sortColumn = request.SortColumn,
    //                 sortOrder = request.SortOrder,
    //                 page = request.Page - 1,
    //                 pageSize = request.PageSize
    //             },
    //             "previous-page",
    //             EndpointMethod.GET));
    //     }

    //     if (pagedList.HasNextPage)
    //     {
    //         pagedList.Links.Add(_linkServices.Generate(
    //             EndpointName.Employee.GetAll,
    //             new
    //             {
    //                 statusFilter = request.StatusFilter,
    //                 roleFilter = request.RoleFilter,
    //                 searchTerm = request.SearchTerm,
    //                 sortColumn = request.SortColumn,
    //                 sortOrder = request.SortOrder,
    //                 page = request.Page + 1,
    //                 pageSize = request.PageSize
    //             },
    //             "next-page",
    //             EndpointMethod.GET));
    //     }
    // }
} 