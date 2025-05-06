using API.Extentions;
using API.Infrastructure;
using Application.Features.ShippingInformationFeature.Queries.GetShippingInformationById;
using MediatR;
using SharedKernel.Constants;

namespace API.Endpoints.ShippingInformations;

internal sealed class GetShippingInformationById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/shipping-informations/{shippingInformationId:guid}",
        async (
            Guid shippingInformationId,
            ISender sender) =>
        {
            var result = await sender.Send(new GetShippingInformationByIdQuery(shippingInformationId));
            return result.Match(Results.Ok, CustomResults.Problem);
        }).WithTags(EndpointTags.ShippingInformations)
        .WithName(EndpointName.ShippingInformations.GetById)
        .Produces(StatusCodes.Status200OK)
        .AddBadRequestResponse()
        .AddForbiddenResponse()
        .AddNotFoundResponse()
        .AddUnauthorizedResponse()
        .WithSummary("Lấy thông tin giao hàng theo id")
        .WithDescription("""
            Lấy thông tin giao hàng theo id.
            
            Sample Request:
            
                GET /api/shipping-informations/{shippingInformationId}
            """)
        .WithOpenApi()
        .RequireAuthorization();
    }
}
