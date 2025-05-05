using API.Extentions;
using API.Infrastructure;
using Application.Features.OrderFeature.Queries.GetAllOrderForAdmin;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Orders;

internal sealed class GetAllOrderForAdmin : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/orders/admin", async (
            [FromQuery] string? statusFilter,
            [FromQuery] string? customerFilter,
            [FromQuery] string? searchTerm,
            [FromQuery] string? sortColumn,
            [FromQuery] string? sortOrder,
            [FromQuery] int? page,
            [FromQuery] int? pageSize,
            ISender sender) =>
        {
            var result = await sender.Send(new GetAllOrderForAdminQuery(
                statusFilter,
                customerFilter,
                searchTerm,
                sortColumn,
                sortOrder,
                page,
                pageSize));

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Order)
        .WithName(EndpointName.Order.GetAllForAdmin)
        .AddBadRequestResponse()
        .AddUnauthorizedResponse()
        .AddForbiddenResponse()
        .Produces(StatusCodes.Status200OK)
        .WithSummary("Lấy danh sách đơn hàng cho nhân viên")
        .WithDescription("""
            Cho phép nhân viên lấy danh sách đơn hàng.
              
            Sample Request:
              
                GET /api/orders/admin?statusFilter=Pending&customerFilter=John&searchTerm=123&sortColumn=Date&sortOrder=asc&page=1&pageSize=10
              
            """)
        .WithOpenApi(o =>
        {
            o.Parameters.FirstOrDefault(p => p.Name == "statusFilter")!
                .Description = "Trạng thái đơn hàng (Pending, Completed, Cancelled)";
            o.Parameters.FirstOrDefault(p => p.Name == "customerFilter")!
                .Description = "Tên khách hàng";
            o.Parameters.FirstOrDefault(p => p.Name == "searchTerm")!
                .Description = "Từ khóa tìm kiếm";
            o.Parameters.FirstOrDefault(p => p.Name == "sortColumn")!
                .Description = "Cột sắp xếp (Date, TotalAmount)";
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