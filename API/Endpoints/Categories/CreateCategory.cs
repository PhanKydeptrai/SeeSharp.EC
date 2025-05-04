using API.Extentions;
using API.Infrastructure;
using Application.Features.CategoryFeature.Commands.CreateCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;
using System.ComponentModel.DataAnnotations;

namespace API.Endpoints.Categories;

internal sealed class CreateCategory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/categories", async (
            [FromForm] CreateCategoryRequest request,
            ISender sender) =>
        {
            var command = new CreateCategoryCommand(request.categoryName, request.image);
            var result = await sender.Send(command);
            return result.Match(Results.Ok, CustomResults.Problem);

        })
        .WithTags(EndpointTags.Categories)
        .WithName(EndpointName.Category.Create)
        .Produces(StatusCodes.Status204NoContent)
        .AddBadRequestResponse()
        .AddUnauthorizedResponse()
        .AddForbiddenResponse()
        .DisableAntiforgery()
        .WithSummary("Tạo danh mục mới")
        .WithDescription("""
            Tạo danh mục mới.
            
            Sample Request:

                POST /api/categories
            """)
        .WithOpenApi()
        .RequireAuthorization();
    }

    private class CreateCategoryRequest
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
