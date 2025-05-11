using API.Extentions;
using Application.Features.ProductFeature.Queries.GetAllProduct;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Products;

internal sealed class GetAllProducts : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/products", async (
            [FromQuery] string? filterProductStatus,
            [FromQuery] string? filterCategory,
            [FromQuery] string? searchTerm,
            [FromQuery] string? sortColumn,
            [FromQuery] string? sortOrder,
            [FromQuery] int? page,
            [FromQuery] int? pageSize,
            ISender sender) =>
        {
            var result = await sender.Send(
                new GetAllProductQuery(
                    filterProductStatus,
                    filterCategory,
                    searchTerm,
                    sortColumn,
                    sortOrder,
                    page,
                    pageSize));

            return Results.Ok(result.Value);
        })
        .WithTags(EndpointTags.Product)
        .WithName(EndpointName.Product.GetAll)
        .Produces(StatusCodes.Status200OK)
        .AddBadRequestResponse()
        .AddNotFoundResponse()
        .WithSummary("Lấy tất cả sản phẩm")
        .WithDescription("""
            Lấy tất cả sản phẩm với các tham số tìm kiếm, phân trang và sắp xếp.
             
            Sample Request:
             
                GET /api/products?filterProductStatus={filterProductStatus}&filterCategory={filterCategory}&searchTerm={searchTerm}&sortColumn={sortColumn}&sortOrder={sortOrder}&page={page}&pageSize={pageSize}
             
            """)
        .WithOpenApi(operation =>
        {
            var filterProductStatusParam = operation.Parameters.FirstOrDefault(p => p.Name == "filterProductStatus");
            var filterCategoryParam = operation.Parameters.FirstOrDefault(p => p.Name == "filterCategory");
            var searchTermParam = operation.Parameters.FirstOrDefault(p => p.Name == "searchTerm");
            var sortColumnParam = operation.Parameters.FirstOrDefault(p => p.Name == "sortColumn");
            var sortOrderParam = operation.Parameters.FirstOrDefault(p => p.Name == "sortOrder");
            var pageParam = operation.Parameters.FirstOrDefault(p => p.Name == "page");
            var pageSizeParam = operation.Parameters.FirstOrDefault(p => p.Name == "pageSize");

            if (filterProductStatusParam is not null)
            {
                filterProductStatusParam.Description = "Trạng thái sản phẩm (string)";
            }

            if (filterCategoryParam is not null)
            {
                filterCategoryParam.Description = "ID của danh mục (GUID)";
            }

            return operation;
        })
        .RequireSubscribedOrGuest();
    }
} 