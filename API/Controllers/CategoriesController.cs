using API.Extentions;
using API.Infrastructure;
using Application.Features.CategoryFeature.Commands.CreateCategory;
using Application.Features.CategoryFeature.Commands.DeleteCategory;
using Application.Features.CategoryFeature.Commands.RestoreCategory;
using Application.Features.CategoryFeature.Commands.UpdateCategory;
using Application.Features.CategoryFeature.Queries.GetAllCategory;
using Application.Features.CategoryFeature.Queries.GetCategoryById;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Controllers;

[Route("api/categories")]
[ApiController]
[ApiKey]
public sealed class CategoriesController : ControllerBase
{
    private readonly ISender _sender;
    public CategoriesController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Create a new category
    /// </summary>
    /// <param name="categoryName"></param>
    /// <param name="image"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName(EndpointName.Category.Create)]
    public async Task<IResult> CreateCategory(
        [FromForm] string categoryName,
        [FromForm] IFormFile? image)
    {
        var command = new CreateCategoryCommand(categoryName, image);
        var result = await _sender.Send(command);
        return result.Match(Results.Ok, CustomResults.Problem);
    }

    /// <summary>
    /// Delete a category
    /// </summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    [HttpDelete]
    [ActionName(EndpointName.Category.Delete)]
    public async Task<IResult> DeleteCategory([FromRoute] Guid categoryId)
    {
        var command = new DeleteCategoryCommand(categoryId);
        var result = await _sender.Send(command);
        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    /// <summary>
    /// Get all categories
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="searchTerm"></param>
    /// <param name="sortColumn"></param>
    /// <param name="sortOrder"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet]
    [ActionName(EndpointName.Category.GetAll)]
    public async Task<IResult> GetAllCategories(
        [FromQuery] string? filter,
        [FromQuery] string? searchTerm,
        [FromQuery] string? sortColumn,
        [FromQuery] string? sortOrder,
        [FromQuery] int? page,
        [FromQuery] int? pageSize)
    {
        var result = await _sender.Send(
            new GetAllCategoryQuery(
                filter,
                searchTerm,
                sortColumn,
                sortOrder,
                page,
                pageSize));

        return Results.Ok(result.Value);
    }

    /// <summary>
    /// Get a category by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [ActionName(EndpointName.Category.GetById)]
    public async Task<IResult> GetCategoryById([FromRoute] Guid id)
    {
        var result = await _sender.Send(new GetCategoryByIdQuery(id));
        return result.Match(Results.Ok, CustomResults.Problem);
    }

    /// <summary>
    /// Restore a category
    /// </summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    [HttpPatch("{categoryId:guid}/restore")]
    [ActionName(EndpointName.Category.Restore)]
    public async Task<IResult> RestoreCategory([FromRoute] Guid categoryId)
    {
        var result = await _sender.Send(new RestoreCategoryCommand(categoryId));
        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    [HttpPut("{categoryId:guid}")]
    [ActionName(EndpointName.Category.Update)]
    public async Task<IResult> UpdateCategory(
        [FromRoute] Guid categoryId, 
        [FromForm] string categoryName, 
        [FromForm] IFormFile? image)
    {
        var result = await _sender.Send(new UpdateCategoryCommand(categoryId, categoryName, image));
        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    // public void MapEndpoint(IEndpointRouteBuilder app)
    // {
    //     app.MapPut("api/categories/{categoryId:guid}", async (
    //         [FromRoute] Guid categoryId,
    //         [FromForm] string categoryName,
    //         [FromForm] IFormFile? image,
    //         ISender sender) =>
    //     {
    //         var result = await sender.Send(new UpdateCategoryCommand(categoryId, categoryName, image));
    //         return result.Match(Results.NoContent, CustomResults.Problem);
    //     })
    //     .DisableAntiforgery()
    //     .WithTags(EndpointTag.Category)
    //     .WithName(EndpointName.Category.Update)
    //     .RequireAuthorization()
    //     .AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>();
    // }
}
