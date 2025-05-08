using API.Extentions;
using API.Infrastructure;
using Application.Features.CategoryFeature.Commands.RestoreCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Categories;

internal sealed class RestoreCategory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("api/categories/{categoryId:guid}/restore", async (
            [FromRoute] Guid categoryId,
            ISender sender) =>
        {
            var result = await sender.Send(new RestoreCategoryCommand(categoryId));
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Categories)
        .WithName(EndpointName.Category.Restore)
        .RequireAuthorization()
        .Produces(StatusCodes.Status204NoContent)
        .AddBadRequestResponse()
        .AddNotFoundResponse()
        .AddUnauthorizedResponse()
        .WithSummary("Khôi phục danh mục đã xóa")
        .WithDescription("""
            Cho phép admin khôi phục danh mục đã xóa.
            
            Sample Request:
            
                PATCH /api/admin/categories/{categoryId}/restore
            
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
