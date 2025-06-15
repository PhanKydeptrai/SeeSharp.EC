using API.Extentions;
using API.Infrastructure;
using Application.Features.ShippingInformationFeature.Commands.SetDefaultForShippingInformation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.ShippingInformations;

internal sealed class SetDefaultShippingInformation : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("api/shipping-informations/{shippingInformationId:guid}/set-default",
        async (
            [FromRoute] Guid shippingInformationId,
            ISender sender,
            HttpContext httpContext) =>
        {
            string token = TokenExtentions.GetTokenFromHeader(httpContext)!;
            var claims = TokenExtentions.DecodeJwt(token);
            claims.TryGetValue(CustomJwtRegisteredClaimNames.CustomerId, out var customerId);

            if (customerId is null) return Results.Unauthorized();

            var result = await sender.Send(new SetDefaultForShippingInformationCommand(shippingInformationId, new Guid(customerId)));

            return result.Match(Results.NoContent, CustomResults.Problem);
        }).WithTags(EndpointTags.ShippingInformations)
        .WithName(EndpointName.ShippingInformations.SetDefault) 
        .Produces(StatusCodes.Status204NoContent)
        .AddBadRequestResponse()
        .AddForbiddenResponse()
        .AddNotFoundResponse()
        .AddUnauthorizedResponse()
        .WithSummary("Thiết lập thông tin giao hàng mặc định")
        .WithDescription("""
            Thiết lập một thông tin giao hàng làm mặc định trong sổ địa chỉ.
            Thông tin giao hàng được chọn sẽ được đặt làm mặc định và các thông tin giao hàng khác sẽ không còn là mặc định.
            Sample Request:
            
                PATCH /api/shipping-informations/00000000-0000-0000-0000-000000000001/set-default
            """)
        .WithOpenApi()
        .RequireAuthorization();
    }
}
