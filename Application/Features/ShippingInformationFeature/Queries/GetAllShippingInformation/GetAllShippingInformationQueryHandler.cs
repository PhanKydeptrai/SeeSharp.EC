using Application.Abstractions.Messaging;
using Application.DTOs.ShippingInformation;
using Application.IServices;
using Domain.Entities.Customers;
using SharedKernel;

namespace Application.Features.ShippingInformationFeature.Queries.GetAllShippingInformation;

internal sealed class GetAllShippingInformationQueryHandler : IQueryHandler<GetAllShippingInformationQuery, List<ShippingInformationResponse>>
{
    private readonly IShippingInformationQueryServices _shippingInformationQueryServices;
    public GetAllShippingInformationQueryHandler(
        IShippingInformationQueryServices shippingInformationQueryServices)
    {
        _shippingInformationQueryServices = shippingInformationQueryServices;
    }

    public async Task<Result<List<ShippingInformationResponse>>> Handle(
        GetAllShippingInformationQuery request, 
        CancellationToken cancellationToken)
    {
        return await _shippingInformationQueryServices.GetAllShippingInformation(CustomerId.FromGuid(request.customerId));
    }
}