using API.Extentions;
using API.Infrastructure;
using Application.Features.ShippingInformationFeature.Commands.DeleteShippingInformation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.ShippingInformations;

internal sealed class DeleteShippingInformation : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/shipping-informations/{shippingInformationId:guid}",
        async (
            [FromRoute] Guid shippingInformationId,
            ISender sender) =>
        {
            var result = await sender.Send(new DeleteShippingInformationCommand(shippingInformationId));

            return result.Match(Results.NoContent, CustomResults.Problem);
        }).WithTags(EndpointTags.ShippingInformations)
        .WithName(EndpointName.ShippingInformations.Delete)
        .Produces(StatusCodes.Status204NoContent)
        .AddBadRequestResponse()
        .AddForbiddenResponse()
        .AddNotFoundResponse()
        .AddUnauthorizedResponse()
        .WithSummary("Xóa thông tin giao hàng từ sổ địa chỉ")
        .WithDescription("""
            Xóa thông tin giao hàng từ sổ địa chỉ dựa vào ID.
            Sample Request:
                DELETE /api/shipping-informations/00000000-0000-0000-0000-000000000001
            """)
        .WithOpenApi()
        .RequireAuthorization();
    }
}
