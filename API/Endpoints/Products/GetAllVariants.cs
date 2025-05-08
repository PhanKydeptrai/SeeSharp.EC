using API.Extentions;
using API.Infrastructure;
using Application.Features.ProductFeature.Queries.GetAllVariantQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Products;

internal sealed class GetAllVariants : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/products/variants", async (
            [FromQuery] string? filterProductStatus,
            [FromQuery] string? filterProduct,
            [FromQuery] string? filterCategory,
            [FromQuery] string? searchTerm,
            [FromQuery] string? sortColumn,
            [FromQuery] string? sortOrder,
            [FromQuery] int? page,
            [FromQuery] int? pageSize,
            ISender sender) =>
        {
            var result = await sender.Send(
                new GetAllVariantQuery(
                    filterProductStatus,
                    filterProduct,
                    filterCategory,
                    searchTerm,
                    sortColumn,
                    sortOrder,
                    page,
                    pageSize));

            return Results.Ok(result.Value);
        })
        .WithTags(EndpointTags.Product)
        .WithName(EndpointName.Product.GetAllVariant)
        .Produces(StatusCodes.Status200OK)
        .AddBadRequestResponse()
        .AddNotFoundResponse()
        .WithSummary("Lấy tất cả biến thể sản phẩm")
        .WithDescription("""
            Lấy tất cả biến thể sản phẩm với các tham số tìm kiếm, phân trang và sắp xếp.
              
            Sample Request:
              
                GET /api/products/variants?filterProductStatus={filterProductStatus}&filterProduct={filterProduct}&filterCategory={filterCategory}&searchTerm={searchTerm}&sortColumn={sortColumn}&sortOrder={sortOrder}&page={page}&pageSize={pageSize}
              
            """)
        .WithOpenApi(operation =>
        {
            var filterProductStatusParam = operation.Parameters.FirstOrDefault(p => p.Name == "filterProductStatus");
            var filterProductParam = operation.Parameters.FirstOrDefault(p => p.Name == "filterProduct");
            var filterCategoryParam = operation.Parameters.FirstOrDefault(p => p.Name == "filterCategory");
            var searchTermParam = operation.Parameters.FirstOrDefault(p => p.Name == "searchTerm");
            var sortColumnParam = operation.Parameters.FirstOrDefault(p => p.Name == "sortColumn");
            var sortOrderParam = operation.Parameters.FirstOrDefault(p => p.Name == "sortOrder");
            var pageParam = operation.Parameters.FirstOrDefault(p => p.Name == "page");
            var pageSizeParam = operation.Parameters.FirstOrDefault(p => p.Name == "pageSize");

            if (filterProductStatusParam is not null)
            {
                filterProductStatusParam.Description = "Trạng thái sản phẩm (Còn hàng, Hết hàng)";
            }

            if (filterProductParam is not null)
            {
                filterProductParam.Description = "ID của sản phẩm (GUID)";
            }

            if (filterCategoryParam is not null)
            {
                filterCategoryParam.Description = "ID của danh mục (GUID)";
            }

            return operation;
        });
    }
} 