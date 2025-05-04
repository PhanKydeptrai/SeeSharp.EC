using API.Extentions;
using API.Infrastructure;
using Application.Features.CategoryFeature.Commands.DeleteCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Endpoints.Categories;

internal sealed class DeleteCategory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/categories/{categoryId:guid}", async (
            [FromRoute] Guid categoryId,
            ISender sender) =>
        {
            var command = new DeleteCategoryCommand(categoryId);
            var result = await sender.Send(command);
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Categories)
        .WithName(EndpointName.Category.Delete)
        .Produces(StatusCodes.Status204NoContent)
        .AddBadRequestResponse()
        .AddNotFoundResponse()
        .AddUnauthorizedResponse()
        .WithOpenApi(operation =>
        {
            var categoryIdParam = operation.Parameters.FirstOrDefault(p => p.Name == "categoryId");

            if (categoryIdParam is not null)
            {
                categoryIdParam.Description = "ID của danh mục (GUID)";
            }

            return operation;
        })
        .WithSummary("Xóa một danh mục")
        .WithDescription("""
            Cho phép admin xoá một danh mục.
            
            Sample Request:

                DELETE /api/categories/{categoryId}

            """)
        .RequireAuthorization();
    }
}
