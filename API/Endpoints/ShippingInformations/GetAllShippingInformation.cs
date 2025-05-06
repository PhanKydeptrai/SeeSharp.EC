using API.Extentions;
using API.Infrastructure;
using Application.Features.ShippingInformationFeature.Queries.GetAllShippingInformation;
using MediatR;
using SharedKernel.Constants;

namespace API.Endpoints.ShippingInformations;

internal sealed class GetAllShippingInformation : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/shipping-informations",
        async (
            HttpContext httpContext,
            ISender sender) =>
        {
            string token = TokenExtentions.GetTokenFromHeader(httpContext)!;
            var claims = TokenExtentions.DecodeJwt(token);
            claims.TryGetValue(CustomJwtRegisteredClaimNames.CustomerId, out var customerId);

            var result = await sender.Send(new GetAllShippingInformationQuery(Guid.Parse(customerId!))); 

            return result.Match(Results.Ok, CustomResults.Problem);

        }).WithTags(EndpointTags.ShippingInformations)
        .WithName(EndpointName.ShippingInformations.GetAll)
        .Produces(StatusCodes.Status200OK)
        .AddBadRequestResponse()
        .AddForbiddenResponse()
        .AddUnauthorizedResponse()
        .WithSummary("Lấy danh sách thông tin vận chuyển cho sổ địa chỉ")
        .WithDescription("""
            Lấy danh sách thông tin vận chuyển cho sổ địa chỉ theo customerId
            
            Sample Request:
                GET /api/shipping-informations
            
            """)
        .WithOpenApi()
        .RequireAuthorization()
        .DisableAntiforgery();

    }
}
