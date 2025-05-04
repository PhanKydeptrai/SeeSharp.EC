using API.Extentions;
using API.Infrastructure;
using Application.Features.CategoryFeature.Queries.GetCategoryById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Categories;

internal sealed class GetCategoryForAdmin : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/admin/categories/{categoryId:guid}", async (
            [FromRoute] Guid categoryId,
            ISender sender) =>
        {
            var result = await sender.Send(new GetCategoryByIdQuery(categoryId));
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Categories)
        .WithName(EndpointName.Category.GetByIdForAdmin)
        .Produces(StatusCodes.Status200OK)
        .AddBadRequestResponse()
        .AddNotFoundResponse()
        .AddUnauthorizedResponse()
        .WithSummary("Lấy thông tin danh mục theo ID")
        .WithDescription("""
            Cho phép admin lấy thông tin danh mục theo ID.
            
            Sample Request:
            
                GET /api/admin/categories/{categoryId}
            
            """)
        .WithOpenApi(operation =>
        {
            var categoryIdParam = operation.Parameters.FirstOrDefault(p => p.Name == "categoryId");

            if (categoryIdParam is not null)
            {
                categoryIdParam.Description = "ID của danh mục (GUID)";
            }

            return operation;
        })
        .RequireAuthorization();
    }
}
