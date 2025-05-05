using API.Extentions;
using API.Infrastructure;
using Application.Features.OrderFeature.Queries.GetCartInformation;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using SharedKernel.Constants;

namespace API.Endpoints.Orders;

internal sealed class GetCartInformation : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/orders/cart", async (
            HttpContext httpContext,
            ISender sender) =>
        {
            string token = TokenExtentions.GetTokenFromHeader(httpContext)!;
            var claims = TokenExtentions.DecodeJwt(token);
            string? id = string.Empty;
            claims.TryGetValue(CustomJwtRegisteredClaimNames.CustomerId, out id);

            if (id.IsNullOrEmpty())
            {
                claims.TryGetValue(CustomJwtRegisteredClaimNames.GuestId, out id);
            }
            var result = await sender.Send(new GetCartInformationQuery(new Guid(id!)));
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Order)
        .WithName(EndpointName.Order.GetCartInformation)
        .Produces(StatusCodes.Status200OK)
        .AddBadRequestResponse()
        .AddNotFoundResponse()
        .AddUnauthorizedResponse()
        .WithSummary("Lấy thông tin giỏ hàng")
        .WithDescription("""
            Cho phép khách hàng lấy thông tin giỏ hàng của mình.
              
            Sample Request:
              
                GET /api/orders/cart
              
            """)
        .WithOpenApi()
        .RequireAuthorization();
    }
} 