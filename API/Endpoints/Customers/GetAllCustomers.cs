using API.Extentions;
using API.Infrastructure;
using Application.Features.CustomerFeature.Queries.GetAllCustomer;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Customers;

internal sealed class GetAllCustomers : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/customers", async (
            [FromQuery] string? statusFilter,
            [FromQuery] string? customerTypeFilter,
            [FromQuery] string? searchTerm,
            [FromQuery] string? sortColumn,
            [FromQuery] string? sortOrder,
            [FromQuery] int? page,
            [FromQuery] int? pageSize,
            ISender sender) =>
        {
            var result = await sender.Send(
                new GetAllCustomerQuery(
                    statusFilter,
                    customerTypeFilter,
                    searchTerm,
                    sortColumn,
                    sortOrder,
                    page,
                    pageSize));

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Customer)
        .WithName(EndpointName.Customer.GetAll)
        .Produces(StatusCodes.Status200OK)
        .WithSummary("Lấy danh sách khách hàng")
        .WithDescription("""
            Cho phép nhân viên lấy danh sách khách hàng.
             
            Sample Request:
             
                GET /api/customers?statusFilter=Active&customerTypeFilter=VIP&searchTerm=John&sortColumn=Name&sortOrder=asc&page=1&pageSize=10
             
            """)
        .WithOpenApi(o =>
        {
            o.Parameters.FirstOrDefault(p => p.Name == "statusFilter")!
                .Description = "Trạng thái khách hàng (Active, Inactive)";
            o.Parameters.FirstOrDefault(p => p.Name == "customerTypeFilter")!
                .Description = "Loại khách hàng (VIP, Regular)";
            o.Parameters.FirstOrDefault(p => p.Name == "searchTerm")!
                .Description = "Từ khóa tìm kiếm";
            o.Parameters.FirstOrDefault(p => p.Name == "sortColumn")!
                .Description = "Cột sắp xếp (Name, Email, PhoneNumber)";
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