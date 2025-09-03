
using API.Extentions;
using API.Infrastructure;
using Application.DTOs.Payment;
using Application.Features.OrderFeature.Commands.VnPayReturnUrl;
using Domain.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Orders;

public sealed class VnPayReturnUrl : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/orders/return-url", async (
            [FromQuery(Name = "vnp_Amount")] string vnp_Amount,
            [FromQuery(Name = "vnp_BankCode")] string vnp_BankCode,
            [FromQuery(Name = "vnp_OrderInfo")] string vnp_OrderInfo,
            [FromQuery(Name = "vnp_ResponseCode")] string vnp_ResponseCode,
            [FromQuery(Name = "vnp_TmnCode")] string vnp_TmnCode,
            [FromQuery(Name = "vnp_TransactionNo")] string vnp_TransactionNo,
            [FromQuery(Name = "vnp_TxnRef")] string vnp_TxnRef,
            [FromQuery(Name = "vnp_SecureHash")] string vnp_SecureHash,
            ISender sender) =>
        {
            var model = new VnPayReturnModel
            {
                vnp_Amount = vnp_Amount,
                vnp_BankCode = vnp_BankCode,
                vnp_OrderInfo = vnp_OrderInfo,
                vnp_ResponseCode = vnp_ResponseCode,
                vnp_TmnCode = vnp_TmnCode,
                vnp_TransactionNo = vnp_TransactionNo,
                vnp_TxnRef = vnp_TxnRef,
                vnp_SecureHash = vnp_SecureHash
            };

            var result = await sender.Send(new VnPayReturnUrlCommand(Guid.Parse(model.vnp_TxnRef)));
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Order);
    }
}
