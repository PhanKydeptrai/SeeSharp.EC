using API.Extentions;
using API.Infrastructure;
using Application.Features.EmployeeFeature.Commands.EmployeeChangePassword;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;
using System.IdentityModel.Tokens.Jwt;

namespace API.Endpoints.Employees;

internal sealed class EmployeeChangePassword : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/employees/change-password", async (
            [FromBody] EmployeeChangePasswordRequest request,
            HttpContext httpContext,
            ISender sender) =>
        {
            string token = TokenExtentions.GetTokenFromHeader(httpContext)!;
            var claims = TokenExtentions.DecodeJwt(token);
            claims.TryGetValue(JwtRegisteredClaimNames.Sub, out var sub);
        
            var result = await sender.Send(new EmployeeChangePasswordCommand(
                Guid.Parse(sub!),
                request.currentPassword,
                request.newPassword,
                request.repeatNewPassword));

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Employee)
        .WithName(EndpointName.Employee.ChangePassword)
        .Produces(StatusCodes.Status204NoContent)
        .AddBadRequestResponse()
        .AddForbiddenResponse()
        .AddUnauthorizedResponse()
        .WithSummary("Thay đổi mật khẩu nhân viên")
        .WithDescription("""
            Cho phép nhân viên thay đổi mật khẩu của mình.
             
            Sample Request:
             
                POST /api/employees/change-password
                {
                    "currentPassword": "oldpassword",
                    "newPassword": "newpassword",
                    "repeatNewPassword": "newpassword"
                }
            """)
        .WithOpenApi()
        .RequireAuthorization();

    }

    private class EmployeeChangePasswordRequest
    {
        /// <summary>
        /// Mật khẩu hiện tại của nhân viên
        /// </summary>
        public string currentPassword { get; set; } = string.Empty;

        /// <summary>
        /// Mật khẩu mới của nhân viên
        /// </summary>
        public string newPassword { get; set; } = string.Empty;

        /// <summary>
        /// Mật khẩu mới của nhân viên (nhập lại để xác nhận)
        /// </summary>
        public string repeatNewPassword { get; set; } = string.Empty;
    }
} 