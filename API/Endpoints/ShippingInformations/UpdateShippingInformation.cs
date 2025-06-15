using API.Extentions;
using API.Infrastructure;
using Application.Features.ShippingInformationFeature.Commands.UpdateShippingInformation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.ShippingInformations;

internal sealed class UpdateShippingInformation : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/shipping-informations/{shippingInformationId:guid}", async (
            [FromRoute] Guid shippingInformationId,
            [FromBody] UpdateShippingInformationRequest request,
            ISender sender) =>
        {
            var result = await sender.Send(new UpdateShippingInformationCommand(
                request.CustomerId,
                shippingInformationId,
                request.FullName,
                request.PhoneNumber,
                request.Province,
                request.District,
                request.IsDefault,
                request.Ward,
                request.SpecificAddress));

            return result.Match(Results.NoContent, CustomResults.Problem);
        }).WithTags(EndpointTags.ShippingInformations)
        .WithName(EndpointName.ShippingInformations.Update)
        .Produces(StatusCodes.Status204NoContent)
        .AddBadRequestResponse()
        .AddForbiddenResponse()
        .AddNotFoundResponse()
        .AddUnauthorizedResponse()
        .WithSummary("Cập nhật thông tin giao hàng trong sổ địa chỉ")
        .WithDescription("""
            Cập nhật thông tin giao hàng trong sổ địa chỉ.
            Sample Request:
            
                PUT /api/shipping-informations/00000000-0000-0000-0000-000000000001
                {
                    "fullName": "Nguyễn Văn A",
                    "phoneNumber": "0123456789",
                    "province": "Hà Nội",
                    "district": "Hoàn Kiếm",
                    "ward": "Phường Tràng Tiền",
                    "specificAddress": "Số 1, phố Tràng Tiền",
                    "isDefault": true
                }
            """)
        .WithOpenApi()
        .RequireAuthorization();
    }

    private class UpdateShippingInformationRequest
    {
        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public Guid CustomerId { get; init; }

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

        /// <summary>
        /// Đặt làm địa chỉ mặc định
        /// </summary>
        public bool IsDefault { get; init; }
    }
}
