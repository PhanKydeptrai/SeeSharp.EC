using API.Extentions;
using API.Infrastructure;
using Application.Features.VoucherFeature.Queries.GetAllVoucher;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Vouchers;

internal sealed class GetAllVouchers : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/vouchers", async (
            [FromQuery] string? voucherTypeFilter,
            [FromQuery] string? statusFilter,
            [FromQuery] string? searchTerm,
            [FromQuery] string? sortColumn,
            [FromQuery] string? sortOrder,
            [FromQuery] int? page,
            [FromQuery] int? pageSize,
            ISender sender) =>
        {
            var result = await sender.Send(new GetAllVoucherQuery(
                voucherTypeFilter,
                statusFilter,
                searchTerm,
                sortColumn,
                sortOrder,
                page,
                pageSize));
                
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Voucher)
        .WithName(EndpointName.Voucher.GetAll)
        .Produces(StatusCodes.Status200OK)
        .AddUnauthorizedResponse()
        .AddForbiddenResponse()
        .WithSummary("Lấy danh sách voucher")
        .WithDescription("""
            Cho phép nhân viên lấy danh sách voucher.
              
            Sample Request:
              
                GET /api/vouchers?voucherTypeFilter=Discount&statusFilter=Active&searchTerm=Summer&sortColumn=Name&sortOrder=asc&page=1&pageSize=10
              
            """)
        .WithOpenApi(o =>
        {
            o.Parameters.FirstOrDefault(p => p.Name == "voucherTypeFilter")!
                .Description = "Loại voucher (Discount, Gift)";
            o.Parameters.FirstOrDefault(p => p.Name == "statusFilter")!
                .Description = "Trạng thái voucher (Active, Inactive)";
            o.Parameters.FirstOrDefault(p => p.Name == "searchTerm")!
                .Description = "Từ khóa tìm kiếm";
            o.Parameters.FirstOrDefault(p => p.Name == "sortColumn")!
                .Description = "Cột sắp xếp (Name, Code, Type)";
            o.Parameters.FirstOrDefault(p => p.Name == "sortOrder")!
                .Description = "Thứ tự sắp xếp (asc, desc)";
            o.Parameters.FirstOrDefault(p => p.Name == "page")!
                .Description = "Số trang";
            o.Parameters.FirstOrDefault(p => p.Name == "pageSize")!
                .Description = "Kích thước trang";

            return o;
        })
        .RequireAuthorization();
    }
} 