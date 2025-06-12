using API.Extentions;
using API.Infrastructure;
using Application.Features.OrderFeature.Commands.MakePaymentForGuest;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Orders;

internal sealed class MakePaymentForGuest : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/orders/make-payment-guest",
        async (
            [FromBody] MakePaymentForGuestRequest request,
            HttpContext httpContext,
            ISender sender) =>
        {
            string? token = TokenExtentions.GetTokenFromHeader(httpContext);
            var claims = TokenExtentions.DecodeJwt(token!);
            claims.TryGetValue(CustomJwtRegisteredClaimNames.GuestId, out var guestId);

            var result = await sender.Send(new MakePaymentForGuestCommand(
                new Guid(guestId!),
                request.Email,
                request.FullName,
                request.PhoneNumber,
                request.Province,
                request.District,
                request.Ward,
                request.SpecificAddress));

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Order)
        .WithName(EndpointName.Order.MakePaymentForGuest)
        .Produces(StatusCodes.Status204NoContent)
        .AddBadRequestResponse()
        .AddNotFoundResponse()
        .DisableAntiforgery()
        .AddUnauthorizedResponse()
        .WithSummary("Thanh toán đơn hàng cho khách hàng chưa đăng nhập").WithDescription("""
            Thực hiện thanh toán đơn hàng cho khách hàng chưa đăng nhập (Guest). 
            Endpoint này cho phép khách hàng chưa có tài khoản thực hiện thanh toán đơn hàng.
            
            Yêu cầu:
            - Token JWT với GuestId hợp lệ
            - Thông tin giao hàng đầy đủ (tên, SĐT, email, địa chỉ)
            
            Lưu ý:
            - Khách Guest không thể sử dụng voucher
            - Khách Guest không có thông tin shipping được lưu trữ
            - Tất cả thông tin giao hàng phải được cung cấp trong request
            
            Sample Request:
                POST /api/orders/make-payment-guest
                {
                    "email": "guest@example.com",
                    "fullName": "Nguyễn Văn A",
                    "phoneNumber": "0123456789",
                    "province": "Hồ Chí Minh",
                    "district": "Quận 1",
                    "ward": "Phường Bến Nghé",
                    "specificAddress": "123 Đường ABC"
                }
        """)
        .WithOpenApi();
    }
}
