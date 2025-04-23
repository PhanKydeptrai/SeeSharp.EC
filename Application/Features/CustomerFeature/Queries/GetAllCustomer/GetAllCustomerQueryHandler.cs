using Application.Abstractions.Messaging;
using Application.DTOs.Customer;
using Application.Features.Pages;
using Application.IServices;
using SharedKernel;

namespace Application.Features.CustomerFeature.Queries.GetAllCustomer;

internal sealed class GetAllCustomerQueryHandler : 
    IQueryHandler<GetAllCustomerQuery, PagedList<CustomerProfileResponse>>
{
    private readonly ICustomerQueryServices _customerQueryServices;
    public GetAllCustomerQueryHandler(ICustomerQueryServices customerQueryServices)
    {
        _customerQueryServices = customerQueryServices;
    }

    public async Task<Result<PagedList<CustomerProfileResponse>>> Handle(
        GetAllCustomerQuery request, 
        CancellationToken cancellationToken)
    {
        var result = await _customerQueryServices.GetAllCustomerProfile(
            request.statusFilter, 
            request.customerTypeFilter,
            request.searchTerm,
            request.sortColumn,
            request.sortOrder,
            request.page,
            request.pageSize);

        return result;
    }
}
