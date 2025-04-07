using System.IdentityModel.Tokens.Jwt;
using API.Extentions;
using API.Infrastructure;
using Application.Features.CustomerFeature.Queries.GetCustomerProfile;
using MediatR;
using SharedKernel.Constants;

namespace API.Endpoints.Customer;

internal sealed class GetCustomerProfile : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/api/customers/profile", async (
            HttpContext httpContext,
            ISender sender) =>
        {
            string token = TokenExtentions.GetTokenFromHeader(httpContext);
            var claims = TokenExtentions.DecodeJwt(token);
            claims.TryGetValue(JwtRegisteredClaimNames.Sub, out var sub);
            var result = await sender.Send(new GetCustomerProfileQuery(new Guid(sub!)));
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .DisableAntiforgery()
        .WithTags(EndpointTags.Customer)
        .WithName(EndpointName.Customer.GetProfile)
        .RequireAuthorization()
        .AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>();
    }
}
