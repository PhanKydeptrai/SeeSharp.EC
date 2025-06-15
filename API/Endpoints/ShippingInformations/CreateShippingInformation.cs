using API.Extentions;
using API.Infrastructure;
using Application.Features.ShippingInformationFeature.Commands.CreateShippingInformation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.ShippingInformations;

internal sealed class CreateShippingInformation : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/shipping-informations", async (
            [FromBody] CreateShippingInformationRequest request,
            HttpContext httpContext,
            ISender sender) =>
        {
            string token = TokenExtentions.GetTokenFromHeader(httpContext)!;
            var claims = TokenExtentions.DecodeJwt(token);
            claims.TryGetValue(CustomJwtRegisteredClaimNames.CustomerId, out var customerId);
            if (customerId is null) return Results.Unauthorized();

            var result = await sender.Send(new CreateShippingInformationCommand(
                new Guid(customerId),
                request.FullName,
                request.PhoneNumber,
                request.Province,
                request.District,
                request.Ward,
                request.SpecificAddress));

            return result.Match(Results.NoContent, CustomResults.Problem);
        }).WithTags(EndpointTags.ShippingInformations)
        .WithName(EndpointName.ShippingInformations.Create)
        .Produces(StatusCodes.Status204NoContent)
        .AddBadRequestResponse()
        .AddForbiddenResponse()
        .AddUnauthorizedResponse()
        .WithSummary("Tạo thông tin giao hàng mới cho sổ địa chỉ")
        .WithDescription("""
            Thêm thông tin giao hàng mới cho sổ địa chỉ.
            Sample Request:

                POST /api/shipping-informations
                {
                    "fullName": "Nguyễn Văn A",
                    "phoneNumber": "0123456789",
                    "province": "Hà Nội",
                    "district": "Hoàn Kiếm",
                    "ward": "Phường Tràng Tiền",
                    "specificAddress": "Số 1, phố Tràng Tiền"
                }
            """)
        .WithOpenApi()
        .RequireAuthorization();
    }

    private class CreateShippingInformationRequest
    {
        /// <summary>
        /// Họ tên
        /// </summary>
        public string FullName { get; init; } = null!;

        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string PhoneNumber { get; init; } = null!;

        /// <summary>
        /// Địa chỉ cụ thể
        /// </summary>
        public string SpecificAddress { get; init; } = null!;

        /// <summary>
        /// Tỉnh/Thành phố
        /// </summary>
        public string Province { get; init; } = null!;

        /// <summary>
        /// Quận/Huyện
        /// </summary>
        public string District { get; init; } = null!;

        /// <summary>
        /// Phường/Xã
        /// </summary>
        public string Ward { get; init; } = null!;
    }
}
