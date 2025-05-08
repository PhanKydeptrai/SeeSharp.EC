using API.Extentions;
using API.Infrastructure;
using Application.Features.CategoryFeature.Commands.UpdateCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;
using System.ComponentModel.DataAnnotations;

namespace API.Endpoints.Categories;

internal sealed class UpdateCategory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/categories/{categoryId:guid}", async (
            [FromRoute] Guid categoryId,
            [FromForm] UpdateCategoryRequest request,
            ISender sender) =>
        {
            var result = await sender.Send(new UpdateCategoryCommand(categoryId, request.categoryName, request.image));
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(EndpointTags.Categories)
        .WithName(EndpointName.Category.Update)
        .Produces(StatusCodes.Status204NoContent)
        .AddBadRequestResponse()
        .AddNotFoundResponse()
        .AddUnauthorizedResponse()
        .AddForbiddenResponse()
        .WithDisplayName("Cập nhật danh mục")
        .WithDescription("""
            Cập nhật danh mục sản phẩm.
            
            Sample Request:
            
                PUT /api/categories/{categoryId}
            
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

    private class UpdateCategoryRequest
    {
        /// <summary>
        /// Tên danh mục
        /// </summary>
        [Required]
        public string categoryName { get; init; } = string.Empty;

        /// <summary>
        /// Ảnh danh mục
        /// </summary>
        public IFormFile? image { get; init; } = null!;
    }
}
