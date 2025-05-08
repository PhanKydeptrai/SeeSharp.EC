using API.Extentions;
using API.Infrastructure;
using Application.Features.CustomerFeature.Queries.GetCustomerProfile;
using MediatR;
using SharedKernel.Constants;
using System.IdentityModel.Tokens.Jwt;

namespace API.Endpoints.Customers;

internal sealed class GetCustomerProfile : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/customers/profile", async (
            HttpContext httpContext,
            ISender sender) =>
        {
            string token = TokenExtentions.GetTokenFromHeader(httpContext)!;
            var claims = TokenExtentions.DecodeJwt(token);
            claims.TryGetValue(JwtRegisteredClaimNames.Sub, out var sub);
            var result = await sender.Send(new GetCustomerProfileQuery(new Guid(sub!)));
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Customer)
        .WithName(EndpointName.Customer.GetProfile)
        .Produces(StatusCodes.Status200OK)
        .AddBadRequestResponse()
        .AddNotFoundResponse()
        .AddUnauthorizedResponse()
        .WithSummary("Lấy thông tin cá nhân của khách hàng")
        .WithDescription("""
            Cho phép khách hàng lấy thông tin cá nhân của mình.
             
            Sample Request:
             
                GET /api/customers/profile
             
            """)
        .WithOpenApi()
        .RequireAuthorization();
    }
} 