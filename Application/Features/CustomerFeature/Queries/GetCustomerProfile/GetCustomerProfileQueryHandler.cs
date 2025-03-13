using Application.Abstractions.Messaging;
using Application.DTOs.Customer;
using Application.IServices;
using Domain.Entities.Users;
using Domain.Utilities.Errors;
using SharedKernel;

namespace Application.Features.CustomerFeature.Queries.GetCustomerProfile;

internal sealed class GetCustomerProfileQueryHandler 
    : IQueryHandler<GetCustomerProfileQuery, CustomerProfileResponse>
{
    private readonly ICustomerQueryServices _customerQueryServices;

    public GetCustomerProfileQueryHandler(ICustomerQueryServices customerQueryServices)
    {
        _customerQueryServices = customerQueryServices;
    }

    public async Task<Result<CustomerProfileResponse>> Handle(
        GetCustomerProfileQuery request, 
        CancellationToken cancellationToken)
    {
        var customerProfile = await _customerQueryServices.GetCustomerProfileById(UserId.FromGuid(request.UserId));
        
        if(customerProfile is null)
        {
            return Result.Failure<CustomerProfileResponse>(CustomerError.NotFoundCustomer());
        }
        
        return Result.Success(customerProfile);
    }
}
