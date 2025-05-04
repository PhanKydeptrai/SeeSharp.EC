using API.Extentions;
using Application.IServices;
using Domain.Entities.Users;
using SharedKernel.Constants;
using System.IdentityModel.Tokens.Jwt;

namespace API.Endpoints.Employees;

internal sealed class GetEmployeeProfile : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/employees/profile", async (
            HttpContext httpContext,
            IEmployeeQueryServices employeeQueryServices) =>
        {
            string token = TokenExtentions.GetTokenFromHeader(httpContext)!;
            var claims = TokenExtentions.DecodeJwt(token);
            claims.TryGetValue(JwtRegisteredClaimNames.Sub, out var sub);

            var profile = await employeeQueryServices.GetEmployeeProfileById(UserId.FromGuid(new Guid(sub!)));
            if (profile is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(profile);
        })
        .WithTags(EndpointTags.Employee)
        .WithName(EndpointName.Employee.GetProfile)
        .Produces(StatusCodes.Status200OK)
        .AddBadRequestResponse()
        .AddForbiddenResponse()
        .AddUnauthorizedResponse()
        .AddNotFoundResponse()
        .WithSummary("Lấy thông tin nhân viên")
        .WithDescription("""
            Cho phép nhân viên lấy thông tin cá nhân của mình.
              
            Sample Request:
              
                GET /api/employees/profile
              
            """)
        .WithOpenApi()
        .RequireAuthorization();
    }
} 