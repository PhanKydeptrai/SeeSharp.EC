using API.Extentions;
using API.Infrastructure;
using Application.Features.CategoryFeature.Queries.GetCategoryInfo;
using MediatR;
using SharedKernel.Constants;

namespace API.Endpoints.Categories;

internal sealed class GetCategoryInfo : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/categories/info", async (
            ISender sender) =>
        {
            var result = await sender.Send(new GetCategoryInfoQuery());
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Categories)
        .WithName(EndpointName.Category.GetCategoryInfo)
        .Produces(StatusCodes.Status200OK)
        .AddBadRequestResponse()
        .WithSummary("Lấy thông tin danh mục")
        .WithDescription("""
            Lấy thông tin danh mục sản phẩm.
            
            Sample Request:
            
                GET /api/categories/info
            
            """)
        .WithOpenApi(operation =>
        {
            var categoryIdParam = operation.Parameters.FirstOrDefault(p => p.Name == "categoryId");

            if (categoryIdParam is not null)
            {
                categoryIdParam.Description = "ID của danh mục (GUID)";
            }

            return operation;
        });
    }
} 