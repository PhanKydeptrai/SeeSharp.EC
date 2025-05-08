using API.Extentions;
using API.Infrastructure;
using Application.Features.VoucherFeature.Queries.GetAllCustomerVoucher;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Vouchers;

internal sealed class GetAllCustomerVouchers : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/vouchers/customer", async (
            [FromQuery] string? voucherTypeFilter,
            [FromQuery] string? statusFilter,
            [FromQuery] string? searchTerm,
            [FromQuery] string? sortColumn,
            [FromQuery] string? sortOrder,
            [FromQuery] int? page,
            [FromQuery] int? pageSize,
            HttpContext httpContext,
            ISender sender) =>
        {
            string token = TokenExtentions.GetTokenFromHeader(httpContext)!;
            var claims = TokenExtentions.DecodeJwt(token);
            claims.TryGetValue(CustomJwtRegisteredClaimNames.CustomerId, out var customerId);

            var result = await sender.Send(new GetAllCustomerVoucherQuery(
                new Guid(customerId!),
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
        .WithName(EndpointName.Voucher.GetAllForCustomer)
        .Produces(StatusCodes.Status200OK)
        .AddUnauthorizedResponse()
        .AddForbiddenResponse()
        .WithSummary("Lấy danh sách voucher của khách hàng")
        .WithDescription("""
            Cho phép khách hàng lấy danh sách voucher của mình.
              
            Sample Request:
              
                GET /api/vouchers/customer?voucherTypeFilter=Discount&statusFilter=Active&searchTerm=Summer&sortColumn=Name&sortOrder=asc&page=1&pageSize=10
              
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
                .Description = "Cột sắp xếp (Name, ExpirationDate)";
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