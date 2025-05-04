using API.Extentions;
using API.Infrastructure;
using Application.Features.CategoryFeature.Queries.GetAllCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Categories;

internal sealed class GetAllCategories : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/categories", async (
            [FromQuery] string? filter,
            [FromQuery] string? searchTerm,
            [FromQuery] string? sortColumn,
            [FromQuery] string? sortOrder,
            [FromQuery] int? page,
            [FromQuery] int? pageSize,
            ISender sender) =>
        {
            var result = await sender.Send(
                new GetAllCategoryQuery(
                    filter,
                    searchTerm,
                    sortColumn,
                    sortOrder,
                    page,
                    pageSize));

            return Results.Ok(result.Value);
        })
        .WithTags(EndpointTags.Categories)
        .WithName(EndpointName.Category.GetAll)
        .Produces(StatusCodes.Status200OK)
        .AddBadRequestResponse()
        .AddNotFoundResponse()
        .AddUnauthorizedResponse()
        .WithSummary("Lấy tất cả danh mục")
        .WithDescription("""
            Lấy tất cả danh mục với các tham số tìm kiếm, phân trang và sắp xếp.
            
            Sample Request:
            
                GET /api/categories?filter={filter}&searchTerm={searchTerm}&sortColumn={sortColumn}&sortOrder={sortOrder}&page={page}&pageSize={pageSize}
            
            """)
        .WithOpenApi(operation =>
        {
            var categoryIdParam = operation.Parameters.FirstOrDefault(p => p.Name == "filter");
            var searchTermParam = operation.Parameters.FirstOrDefault(p => p.Name == "searchTerm");
            var sortColumnParam = operation.Parameters.FirstOrDefault(p => p.Name == "sortColumn");
            var sortOrderParam = operation.Parameters.FirstOrDefault(p => p.Name == "sortOrder");
            var pageParam = operation.Parameters.FirstOrDefault(p => p.Name == "page");
            var pageSizeParam = operation.Parameters.FirstOrDefault(p => p.Name == "pageSize");

            if (categoryIdParam is not null)
            {
                categoryIdParam.Description = "ID của danh mục (GUID)";
            }

            if (searchTermParam is not null)
            {
                searchTermParam.Description = "Từ khóa tìm kiếm";
            }  

            if (sortColumnParam is not null)
            {
                sortColumnParam.Description = "Cột để sắp xếp";
            }

            if (sortOrderParam is not null)
            {
                sortOrderParam.Description = "Thứ tự sắp xếp (asc hoặc desc)";
            }

            if (pageParam is not null)
            {
                pageParam.Description = "Số trang";
            }

            if (pageSizeParam is not null)
            {
                pageSizeParam.Description = "Kích thước trang";
            }

            return operation;
        });
    }
}
