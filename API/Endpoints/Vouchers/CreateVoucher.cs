using API.Extentions;
using API.Infrastructure;
using Application.Features.VoucherFeature.Commands.CreateNewVoucher;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Vouchers;

internal sealed class CreateVoucher : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/vouchers", async (
            [FromBody] CreateNewVoucherCommand command,
            ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Voucher)
        .WithName(EndpointName.Voucher.Create)
        .Produces(StatusCodes.Status204NoContent)
        .AddBadRequestResponse()
        .AddForbiddenResponse()
        .AddUnauthorizedResponse()
        .WithSummary("Tạo mã giảm giá")
        .WithDescription("""
            Cho phép admin tạo mã giảm giá mới.
              
            Sample Request:
              
                POST /api/vouchers
                {
                    "voucherName": "Giảm giá 10%",
                    "voucherCode": "GiamGia10",
                    "voucherType": "Percentage",
                    "percentageDiscount": 10,
                    "maximumDiscountAmount": 1000000,
                    "minimumOrderAmount": 500000,
                    "startDate": "2023-01-01",
                    "expiredDate": "2023-12-31",
                    "voucherDescription": "Giảm giá 10% cho đơn hàng từ 500.000đ"
                }
                
            """)
        .WithOpenApi()
        .RequireAuthorization();
        
    }

    private class CreateNewVoucherRequest
    {
        /// <summary>
        /// Tên mã giảm giá
        /// </summary>
        public string VoucherName { get; init; } = string.Empty;
        
        /// <summary>
        /// Mã giảm giá
        /// </summary>
        public string VoucherCode { get; init; } = string.Empty;
        
        /// <summary>
        /// Loại mã giảm giá (Giảm giá theo phần trăm hoặc theo số tiền)
        /// </summary>
        public string VoucherType { get; init; } = string.Empty;
        
        /// <summary>
        /// Giảm giá theo phần trăm (0-100)
        /// </summary>
        public int PercentageDiscount { get; init; }

        /// <summary>
        /// Giảm giá theo số tiền (0-1000000)
        /// </summary>
        public decimal MaximumDiscountAmount { get; init; }

        /// <summary>
        /// Số lượng mã giảm giá
        /// </summary>
        public decimal MinimumOrderAmount { get; init; }
        
        /// <summary>
        /// Thời gian bắt đầu áp dụng mã giảm giá
        /// </summary>
        public DateOnly StartDate { get; init; }

        /// <summary>
        /// Thời gian hết hạn mã giảm giá
        /// </summary>
        public DateOnly ExpiredDate { get; init; }
        public string VoucherDescription { get; init; } = string.Empty;
        
        
    }
} 