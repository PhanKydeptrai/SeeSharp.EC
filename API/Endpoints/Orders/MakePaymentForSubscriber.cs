using API.Extentions;
using API.Infrastructure;
using Application.Features.OrderFeature.Commands.MakePaymentForSubscriber;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Orders;

internal sealed class MakePaymentForSubscriber : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/orders/make-payment", async (
            [FromBody] MakePaymentForSubscriberRequest request,
            HttpContext httpContext,
            ISender sender) =>
        {
            string? token = TokenExtentions.GetTokenFromHeader(httpContext);
            var claims = TokenExtentions.DecodeJwt(token!);
            claims.TryGetValue(CustomJwtRegisteredClaimNames.CustomerId, out var customerId);

            var result = await sender.Send(new MakePaymentForSubscriberCommand(
                new Guid(customerId!),
                request.voucherCode,
                request.ShippingInformationId,
                request.FullName,
                request.PhoneNumber,
                request.Province,
                request.District,
                request.Ward,
                request.SpecificAddress));

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Order)
        .WithName(EndpointName.Order.MakePaymentForSubscriber)
        .Produces(StatusCodes.Status204NoContent)
        .AddBadRequestResponse()
        .AddForbiddenResponse()
        .AddNotFoundResponse()
        .AddUnauthorizedResponse()
        .WithSummary("Thanh toán đơn hàng cho khách hàng đăng ký")
        .WithDescription("""
            Cho phép khách hàng đăng ký thanh toán đơn hàng của mình.
            
            Sample Request:
            
                POST /api/orders/make-payment
                
            """)
        .WithOpenApi();
    }

    private class MakePaymentForSubscriberRequest
    {
        /// <summary>
        /// Mã giảm giá (nếu có)
        /// </summary>
        public string? voucherCode { get; init; }

        /// <summary>
        /// ID thông tin giao hàng (nếu có)
        /// </summary>
        public Guid? ShippingInformationId { get; init; }

        /// <summary>
        /// Tên người nhận hàng
        /// </summary>
        public string FullName { get; init; } = string.Empty;
        
        /// <summary>
        /// Số điện thoại người nhận hàng
        /// </summary>
        public string PhoneNumber { get; init; } = string.Empty;
        
        /// <summary>
        /// Tỉnh/Thành phố
        /// </summary>
        public string Province { get; init; } = string.Empty;
        
        /// <summary>
        /// Quận/Huyện
        /// </summary>
        public string District { get; init; } = string.Empty;
        
        /// <summary>
        /// Phường/Xã
        /// </summary>
        public string Ward { get; init; } = string.Empty;
        
        /// <summary>
        /// Địa chỉ cụ thể
        /// </summary>
        public string SpecificAddress { get; init; } = string.Empty;
    }
} 