using API.Extentions;
using API.Infrastructure;
using Application.Features.WishItemFeature.Queries.GetWishList;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Wishlists;

internal sealed class GetWishList : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/wishitems", async (
            [FromQuery] string? productStatusFilter,
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

            var query = new GetWishListQuery(
                productStatusFilter,
                searchTerm,
                sortColumn,
                sortOrder,
                page,
                pageSize,
                Guid.Parse(customerId!));

            var result = await sender.Send(query);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Wishlist)
        .WithName(EndpointName.Wishlist.GetWishList)
        .Produces(StatusCodes.Status200OK)
        .AddBadRequestResponse()
        .AddForbiddenResponse()
        .AddUnauthorizedResponse()
        .AddNotFoundResponse()
        .WithSummary("Lấy danh sách sản phẩm yêu thích của khách hàng")
        .WithDescription("""
            Lấy danh sách sản phẩm yêu thích của khách hàng.
                
            Sample Request:
                
                GET /api/wishitems?productStatusFilter=all&searchTerm=product&sortColumn=name&sortOrder=asc&page=1&pageSize=10
                
            """)
        .WithOpenApi(o =>
        {
            o.Parameters.FirstOrDefault(p => p.Name == "productStatusFilter")!.Description = "Trạng thái sản phẩm (all, available, unavailable)";
            o.Parameters.FirstOrDefault(p => p.Name == "searchTerm")!.Description = "Từ khóa tìm kiếm sản phẩm";
            o.Parameters.FirstOrDefault(p => p.Name == "sortColumn")!.Description = "Cột sắp xếp (name, price, createdAt)";
            o.Parameters.FirstOrDefault(p => p.Name == "sortOrder")!.Description = "Thứ tự sắp xếp (asc, desc)";
            o.Parameters.FirstOrDefault(p => p.Name == "page")!.Description = "Số trang (default: 1)";
            o.Parameters.FirstOrDefault(p => p.Name == "pageSize")!.Description = "Kích thước trang (default: 10)";

            return o;
        })
        .RequireAuthorization();
    }
}