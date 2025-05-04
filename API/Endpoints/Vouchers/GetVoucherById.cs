using API.Extentions;
using API.Infrastructure;
using Application.Features.VoucherFeature.Queries.GetVoucherById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Vouchers;

internal sealed class GetVoucherById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/vouchers/{voucherId:guid}", async (
            [FromRoute] Guid voucherId,
            ISender sender) =>
        {
            var result = await sender.Send(new GetVoucherByIdQuery(voucherId));
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Voucher)
        .WithName(EndpointName.Voucher.GetById)
        .Produces(StatusCodes.Status200OK)
        .AddBadRequestResponse()
        .AddNotFoundResponse()
        .WithSummary("Lấy thông tin chi tiết voucher theo ID")
        .WithDescription("""
            Cho phép nhân viên lấy thông tin chi tiết voucher theo ID.
              
            Sample Request:
              
                GET /api/vouchers/{voucherId}
              
            """)
        .WithOpenApi(o =>
        {
            var voucherIdParam = o.Parameters.FirstOrDefault(p => p.Name == "voucherId");

            if (voucherIdParam != null)
            {
                voucherIdParam.Description = "ID của voucher";
                voucherIdParam.Required = true;
            }
            
            return o;
        })
        .RequireAuthorization();
    }
} 