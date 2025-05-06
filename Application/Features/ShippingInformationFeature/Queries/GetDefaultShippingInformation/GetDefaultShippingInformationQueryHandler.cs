using Application.Abstractions.Messaging;
using Application.DTOs.ShippingInformation;
using Application.IServices;
using Domain.Entities.Customers;
using SharedKernel;

namespace Application.Features.ShippingInformationFeature.Queries.GetDefaultShippingInformation;

internal sealed class GetDefaultShippingInformationQueryHandler 
    : IQueryHandler<GetDefaultShippingInformationQuery, ShippingInformationResponse>
{
    private readonly IShippingInformationQueryServices _shippingInformationQueryServices;

    public GetDefaultShippingInformationQueryHandler(IShippingInformationQueryServices shippingInformationQueryServices)
    {
        _shippingInformationQueryServices = shippingInformationQueryServices;
    }

    public async Task<Result<ShippingInformationResponse>> Handle(
        GetDefaultShippingInformationQuery request, 
        CancellationToken cancellationToken)
    {
        var customerId = CustomerId.FromGuid(request.CustomerId);
        var shippingInformation = await _shippingInformationQueryServices
            .GetDefaultShippingInformation(customerId);

        if (shippingInformation is null)
        {
            return Result.Failure<ShippingInformationResponse>(new Error(
                "ShippingInformationNotFound",
                $"Shipping information not found for customer with ID {request.CustomerId}.", 
                ErrorType.NotFound));
        }

        return Result.Success(shippingInformation);
    }
}
