using Application.Abstractions.Messaging;
using Application.DTOs.Customer;
using Application.IServices;
using Domain.Entities.Users;
using Domain.Utilities.Errors;
using SharedKernel;

namespace Application.Features.CustomerFeature.Queries.GetCustomerProfileById;
internal class GetCustomerProfileByIdQueryHandler
    : IQueryHandler<GetCustomerProfileByIdQuery, CustomerProfileResponse>
{
    private readonly ICustomerQueryServices _customerQueryServices;
    public GetCustomerProfileByIdQueryHandler(ICustomerQueryServices customerQueryServices)
    {
        _customerQueryServices = customerQueryServices;
    }

    public async Task<Result<CustomerProfileResponse>> Handle(
        GetCustomerProfileByIdQuery request, 
        CancellationToken cancellationToken)
    {
        var customerProfile = await _customerQueryServices.GetCustomerProfileById(UserId.FromGuid(request.UserId));

        if (customerProfile is null)
        {
            return Result.Failure<CustomerProfileResponse>(CustomerError.NotFoundCustomer());
        }

        return Result.Success(customerProfile);
    }
}


