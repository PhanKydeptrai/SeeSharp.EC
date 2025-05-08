using API.Extentions;
using API.Infrastructure;
using Application.Features.OrderFeature.Queries.GetAllOrderForCustomer;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Orders;

internal sealed class GetAllOrdersForCustomer : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/orders/customer", async (
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

            var query = new GetAllOrderForCustomerQuery(
                new Guid(customerId!),
                statusFilter,
                searchTerm,
                sortColumn,
                sortOrder,
                page,
                pageSize);

            var result = await sender.Send(query);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Order)
        .WithName(EndpointName.Order.GetAllOrderForCustomer)
        .Produces(StatusCodes.Status200OK)
        .AddBadRequestResponse()
        .AddUnauthorizedResponse()
        .AddForbiddenResponse()
        .WithSummary("Lấy danh sách đơn hàng cho khách hàng")
        .WithDescription("""
            Cho phép khách hàng lấy danh sách đơn hàng của mình.
              
            Sample Request:
              
                GET /api/orders/customer?statusFilter=Pending&searchTerm=123&sortColumn=Date&sortOrder=asc&page=1&pageSize=10
              
            """)
        .WithOpenApi(o =>
        {
            o.Parameters.FirstOrDefault(p => p.Name == "statusFilter")!
                .Description = "Trạng thái đơn hàng (Pending, Completed, Cancelled)";
            o.Parameters.FirstOrDefault(p => p.Name == "searchTerm")!
                .Description = "Từ khóa tìm kiếm";
            o.Parameters.FirstOrDefault(p => p.Name == "sortColumn")!
                .Description = "Cột sắp xếp (Date, TotalAmount)";
            o.Parameters.FirstOrDefault(p => p.Name == "sortOrder")!
                .Description = "Thứ tự sắp xếp (asc, desc)";
            return o;
        })
        .RequireAuthorization();
    }
} 