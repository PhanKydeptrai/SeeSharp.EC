using API.Extentions;
using API.Infrastructure;
using Application.Features.CustomerFeature.Commands.UpdateCustomerProfile;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;
using System.IdentityModel.Tokens.Jwt;

namespace API.Endpoints.Customers;

internal sealed class UpdateProfile : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/customers/profile", async (
            [FromForm] UpdateProfileRequest request,
            HttpContext httpContext,
            ISender sender) =>
        {
            string token = TokenExtentions.GetTokenFromHeader(httpContext)!;
            var claims = TokenExtentions.DecodeJwt(token);
            claims.TryGetValue(JwtRegisteredClaimNames.Sub, out var sub);

            var userId = Guid.Parse(sub!);

            var command = new UpdateCustomerProfileCommand(
                userId,
                request.UserName,
                request.PhoneNumber,
                int.Parse(request.Gender),
                request.DateOfBirth != null ? DateOnly.Parse(request.DateOfBirth) : null,
                request.ImageFile
            );

            var result = await sender.Send(command);
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Customer)
        .WithName(EndpointName.Customer.UpdateProfile)
        .Produces(StatusCodes.Status204NoContent)
        .AddBadRequestResponse()
        .AddNotFoundResponse()
        .AddUnauthorizedResponse()
        .WithSummary("Cập nhật thông tin cá nhân của khách hàng")
        .WithDescription("""
            Cho phép khách hàng cập nhật thông tin cá nhân của mình.
             
            Sample Request:
             
                PUT /api/customers/profile
             
            """)
        .WithOpenApi()
        .RequireAuthorization();
    }

    private class UpdateProfileRequest
    {
        /// <summary>
        /// Tên nhân viên
        /// </summary>
        public string UserName { get; set; } = string.Empty;
        /// <summary>
        /// Số điện thoại nhân viên
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Giới tính nhân viên
        /// </summary>
        public string Gender { get; set; } = string.Empty;

        /// <summary>
        /// Ngày sinh nhân viên
        /// </summary>
        public string? DateOfBirth { get; set; } = null;

        /// <summary>
        /// Ảnh đại diện nhân viên
        /// </summary>
        public IFormFile? ImageFile { get; set; } = null;
    }
} 