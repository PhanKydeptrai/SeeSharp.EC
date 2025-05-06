using Application.Abstractions.Messaging;
using Application.DTOs.ShippingInformation;
using Application.IServices;
using Domain.Entities.ShippingInformations;
using SharedKernel;

namespace Application.Features.ShippingInformationFeature.Queries.GetShippingInformationById;

internal sealed class GetShippingInformationByIdQueryHandler : IQueryHandler<GetShippingInformationByIdQuery, ShippingInformationResponse>
{
    private readonly IShippingInformationQueryServices _shippingInformationQueryServices;

    public GetShippingInformationByIdQueryHandler(IShippingInformationQueryServices shippingInformationQueryServices)
    {
        _shippingInformationQueryServices = shippingInformationQueryServices;
    }

    public async Task<Result<ShippingInformationResponse>> Handle(GetShippingInformationByIdQuery request, CancellationToken cancellationToken)
    {
        var shippingInformation = await _shippingInformationQueryServices.GetShippingInformationById(
            ShippingInformationId.FromGuid(request.ShippingInformationId));

        if (shippingInformation is null)
        {
            return Result.Failure<ShippingInformationResponse>(new Error(
                "ShippingInformationNotFound",
                $"Shipping information not found with ID {request.ShippingInformationId}.",
                ErrorType.NotFound));
        }

        return Result.Success(shippingInformation);
    }
}