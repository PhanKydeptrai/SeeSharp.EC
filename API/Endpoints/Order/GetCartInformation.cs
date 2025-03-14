using API.Extentions;
using API.Infrastructure;
using Application.Features.OrderFeature.Queries.GetCartInformation;
using MediatR;
using SharedKernel.Constants;

namespace API.Endpoints.Order;

internal sealed class GetCartInformation : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet("api/orders/cart", async (
            HttpContext httpContext,
            ISender sender) =>
        {
            string token = TokenExtentions.GetTokenFromHeader(httpContext);
            var claims = TokenExtentions.DecodeJwt(token);
            claims.TryGetValue("CustomerId", out var customerId);

            var result = await sender.Send(new GetCartInformationQuery(new Guid(customerId!)));
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .DisableAntiforgery()
        .RequireAuthorization()
        .WithTags(EndpointTag.Order)
        .WithName(EndpointName.Order.GetCartInformation)
        .AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>();
    }
}
