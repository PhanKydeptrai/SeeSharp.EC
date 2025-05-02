using API.Extentions;
using API.Infrastructure;
using Application.Features.CategoryFeature.Commands.CreateCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;
using System.ComponentModel.DataAnnotations;

namespace API.Endpoints.Categories;

public record CreateCategoryRequest
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

internal sealed class CreateCategory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/categories",
        async (
            [FromForm] CreateCategoryRequest request,
            ISender sender) =>
        {
            var command = new CreateCategoryCommand(request.categoryName, request.image);
            var result = await sender.Send(command);
            return result.Match(Results.Ok, CustomResults.Problem);

        }).WithTags(EndpointTags.Category)
        .WithName(EndpointName.Category.Create)
        .Produces(204)
        .DisableAntiforgery()
        .ProducesProblem(400)
        .WithOpenApi(operation =>
        {
            operation.Summary = "Tạo một danh mục mới";
            operation.Description = """
            Tạo danh mục mới.
            
            Sample Request:

                POST /api/categories
                {
                    "categoryName": "Danh mục 1",
                    "image": "base64string"
                }
            """;
            return operation;
        }).RequireAuthorization();
    }
}
