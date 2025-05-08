using API.Extentions;
using API.Infrastructure;
using Application.Features.EmployeeFeature.Queries.GetAllEmployees;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Employees;

internal sealed class GetAllEmployees : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/employees", async (
            [FromQuery] string? statusFilter,
            [FromQuery] string? roleFilter,
            [FromQuery] string? searchTerm,
            [FromQuery] string? sortColumn,
            [FromQuery] string? sortOrder,
            [FromQuery] int? page,
            [FromQuery] int? pageSize,
            ISender sender) =>
        {
            var result = await sender.Send(
                new GetAllEmployeesQuery(
                    statusFilter,
                    roleFilter,
                    searchTerm,
                    sortColumn,
                    sortOrder,
                    page,
                    pageSize));

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Employee)
        .WithName(EndpointName.Employee.GetAll)
        .Produces(StatusCodes.Status200OK)
        .AddBadRequestResponse()
        .AddForbiddenResponse()
        .AddUnauthorizedResponse()
        .WithSummary("Lấy danh sách nhân viên")
        .WithDescription("""
            Cho phép admin lấy danh sách nhân viên với các bộ lọc và phân trang.
             
            Sample Request:
             
                GET /api/employees?statusFilter=active&roleFilter=admin&searchTerm=john&sortColumn=name&sortOrder=asc&page=1&pageSize=10
             
            """)
        .WithOpenApi(o =>
        {
            o.Parameters.FirstOrDefault(p => p.Name == "statusFilter")!.Description = "Trạng thái nhân viên (active, inactive)";
            o.Parameters.FirstOrDefault(p => p.Name == "roleFilter")!.Description = "Vai trò nhân viên (admin, employee)";
            o.Parameters.FirstOrDefault(p => p.Name == "searchTerm")!.Description = "Từ khóa tìm kiếm trong tên hoặc email nhân viên";
            o.Parameters.FirstOrDefault(p => p.Name == "sortColumn")!.Description = "Cột để sắp xếp (name, email, phoneNumber)";
            o.Parameters.FirstOrDefault(p => p.Name == "sortOrder")!.Description = "Thứ tự sắp xếp (asc, desc)";
            o.Parameters.FirstOrDefault(p => p.Name == "page")!.Description = "Số trang (mặc định là 1)";
            o.Parameters.FirstOrDefault(p => p.Name == "pageSize")!.Description = "Số lượng nhân viên trên mỗi trang (mặc định là 10)";
            return o;
        })
        .RequireAuthorization();
    }
}