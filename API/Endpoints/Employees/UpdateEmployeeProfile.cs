using API.Extentions;
using API.Infrastructure;
using Application.Features.EmployeeFeature.Commands.UpdateEmployeeProfile;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;
using System.IdentityModel.Tokens.Jwt;

namespace API.Endpoints.Employees;

internal sealed class UpdateEmployeeProfile : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/employees/profile", async (
            [FromForm] UpdateEmployeeProfileRequest request,
            HttpContext httpContext,
            ISender sender) =>
        {
            string token = TokenExtentions.GetTokenFromHeader(httpContext)!;
            var claims = TokenExtentions.DecodeJwt(token);
            claims.TryGetValue(JwtRegisteredClaimNames.Sub, out var sub);

            var command = new UpdateEmployeeProfileCommand(
                new Guid(sub!),
                request.UserName,
                request.PhoneNumber,
                request.DateOfBirth,
                request.Avatar);

            var result = await sender.Send(command);
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Employee)
        .WithName(EndpointName.Employee.UpdateProfile)
        .Produces(StatusCodes.Status204NoContent)
        .AddBadRequestResponse()
        .AddNotFoundResponse()
        .AddUnauthorizedResponse()
        .WithSummary("Cập nhật thông tin nhân viên")
        .WithDescription("""
            Cho phép nhân viên cập nhật thông tin cá nhân của mình.
            
            Sample Request:
            
                PUT /api/employees/profile
            
            """)
        .WithOpenApi()
        .RequireAuthorization();
    }

    private class UpdateEmployeeProfileRequest
    {
        /// <summary>
        /// Tên nhân viên
        /// </summary>
        public string UserName { get; init; } = string.Empty;

        /// <summary>
        /// Số điện thoại nhân viên
        /// </summary>
        public string PhoneNumber { get; init; } = string.Empty;

        /// <summary>
        /// Ngày sinh nhân viên
        /// </summary>
        public DateOnly? DateOfBirth { get; init; } = null;
        
        /// <summary>
        /// Ảnh đại diện nhân viên
        /// </summary>
        public IFormFile? Avatar { get; init; } = null;
    }
} 