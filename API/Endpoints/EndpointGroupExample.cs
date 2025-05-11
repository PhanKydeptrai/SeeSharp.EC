using API.Infrastructure.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints;

/// <summary>
/// Ví dụ về cách sử dụng nhóm endpoint với phân quyền
/// Đây chỉ là ví dụ, không phải là endpoint thực tế
/// </summary>
internal sealed class EndpointGroupExample : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        // Nhóm endpoint chỉ dành cho Admin
        app.MapAdminGroup("api/admin", group =>
        {
            group.MapGet("dashboard", () => Results.Ok(new { message = "Admin Dashboard" }))
                .WithTags("Example")
                .WithName("AdminDashboard");

            group.MapPost("settings", () => Results.NoContent())
                .WithTags("Example")
                .WithName("AdminSettings");
        });

        // Nhóm endpoint dành cho Employee (bao gồm cả Admin)
        app.MapEmployeeGroup("api/employee", group =>
        {
            group.MapGet("tasks", () => Results.Ok(new { message = "Employee Tasks" }))
                .WithTags("Example")
                .WithName("EmployeeTasks");
                
            group.MapPost("reports", () => Results.NoContent())
                .WithTags("Example")
                .WithName("EmployeeReports");
        });

        // Nhóm endpoint dành cho khách hàng đã đăng ký
        app.MapSubscribedGroup("api/customer", group =>
        {
            group.MapGet("profile", () => Results.Ok(new { message = "Customer Profile" }))
                .WithTags("Example")
                .WithName("CustomerProfile");
                
            group.MapPost("orders", () => Results.NoContent())
                .WithTags("Example")
                .WithName("CustomerOrders");
        });

        // Nhóm endpoint dành cho cả khách hàng đã đăng ký và khách vãng lai
        app.MapSubscribedOrGuestGroup("api/public", group =>
        {
            group.MapGet("products", () => Results.Ok(new { message = "Public Products" }))
                .WithTags("Example")
                .WithName("PublicProducts");
                
            group.MapGet("categories", () => Results.Ok(new { message = "Public Categories" }))
                .WithTags("Example")
                .WithName("PublicCategories");
        });
    }
} 