using API.Extentions;
using API.Infrastructure;
using Application.Features.ShippingInformationFeature.Queries.GetDefaultShippingInformation;
using MediatR;
using SharedKernel.Constants;

namespace API.Endpoints.ShippingInformations;

internal sealed class GetDefaultShippingInformation : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/shipping-informations/default", async (
            HttpContext httpContext,
            ISender sender) =>
        {
            string? token = TokenExtentions.GetTokenFromHeader(httpContext);
            var claims = TokenExtentions.DecodeJwt(token!);
            claims.TryGetValue(CustomJwtRegisteredClaimNames.CustomerId, out var customerId);
            var result = await sender.Send(new GetDefaultShippingInformationQuery(Guid.Parse(customerId!)));
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(EndpointTags.ShippingInformations)
        .WithName(EndpointName.ShippingInformations.GetDefaultShippingInformation)
        .Produces(StatusCodes.Status200OK)
        .AddBadRequestResponse()
        .AddForbiddenResponse()
        .AddNotFoundResponse()
        .AddUnauthorizedResponse()
        .WithSummary("Lấy thông tin giao hàng mặc định")
        .WithDescription("""
            Lấy thông tin giao hàng mặc định.
            
            Sample Request:
            
                GET /api/shipping-informations/default

            """)
        .WithOpenApi()
        .RequireAuthorization();
    }
}
