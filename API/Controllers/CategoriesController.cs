using API.Extentions;
using API.Infrastructure;
using Application.Features.CategoryFeature.Commands.CreateCategory;
using Application.Features.CategoryFeature.Commands.DeleteCategory;
using Application.Features.CategoryFeature.Commands.RestoreCategory;
using Application.Features.CategoryFeature.Commands.UpdateCategory;
using Application.Features.CategoryFeature.Queries.GetAllCategory;
using Application.Features.CategoryFeature.Queries.GetCategoryById;
using Application.Features.CategoryFeature.Queries.GetCategoryInfo;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace API.Controllers;

[Route("api/categories")]
[ApiController]
public sealed class CategoriesController : ControllerBase
{
    private readonly ISender _sender;
    public CategoriesController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Tạo một danh mục mới
    /// </summary>
    /// <param name="categoryName"></param>
    /// <param name="image"></param>
    /// <returns></returns>
    //[HttpPost]
    //[EndpointName(EndpointName.Category.Create)]
    //public async Task<IResult> CreateCategory(
    //    [FromForm] string categoryName,
    //    IFormFile? image)
    //{
    //    var command = new CreateCategoryCommand(categoryName, image);
    //    var result = await _sender.Send(command);
    //    return result.Match(Results.Ok, CustomResults.Problem);
    //}

    /// <summary>
    /// Xóa một danh mục
    /// </summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    [HttpDelete("{categoryId:guid}")]
    [EndpointName(EndpointName.Category.Delete)]
    public async Task<IResult> DeleteCategory([FromRoute] Guid categoryId)
    {
        var command = new DeleteCategoryCommand(categoryId);
        var result = await _sender.Send(command);
        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    /// <summary>
    /// Lấy tất cả các danh mục
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="searchTerm"></param>
    /// <param name="sortColumn"></param>
    /// <param name="sortOrder"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet]
    [EndpointName(EndpointName.Category.GetAll)]
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
    /// Lấy một danh mục theo id
    /// </summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    [HttpGet("{categoryId:guid}")]
    [EndpointName(EndpointName.Category.GetById)]
    public async Task<IResult> GetCategoryById([FromRoute] Guid categoryId)
    {
        var result = await _sender.Send(new GetCategoryByIdQuery(categoryId));
        return result.Match(Results.Ok, CustomResults.Problem);
    }

    /// <summary>
    /// Lấy một danh mục theo id cho admin, lấy bất kỳ trạng thái nào của danh mục
    /// </summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>/api/admin/categories/{categoryId:guid}")]
    [HttpGet]
    [Route("~/api/admin/categories/{categoryId:guid}")]
    [EndpointName(EndpointName.Category.GetByIdForAdmin)]
    public async Task<IResult> GetCategoryForAdmin([FromRoute] Guid categoryId)
    {
        var result = await _sender.Send(new GetCategoryByIdQuery(categoryId));
        return result.Match(Results.Ok, CustomResults.Problem);
    }
    
    /// <summary>
    /// Khôi phục một danh mục
    /// </summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    [HttpPatch("{categoryId:guid}/restore")]
    [EndpointName(EndpointName.Category.Restore)]
    public async Task<IResult> RestoreCategory([FromRoute] Guid categoryId)
    {
        var result = await _sender.Send(new RestoreCategoryCommand(categoryId));
        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    /// <summary>
    /// Cập nhật một danh mục
    /// </summary>
    /// <param name="categoryId"></param>
    /// <param name="categoryName"></param>
    /// <param name="image"></param>
    /// <returns></returns>
    [HttpPut("{categoryId:guid}")]
    [EndpointName(EndpointName.Category.Update)]
    public async Task<IResult> UpdateCategory(
        [FromRoute] Guid categoryId, 
        [FromForm] string categoryName, 
        IFormFile? image)
    {
        var result = await _sender.Send(new UpdateCategoryCommand(categoryId, categoryName, image));
        return result.Match(Results.NoContent, CustomResults.Problem);
    }

    /// <summary>
    /// Lấy thông tin danh mục
    /// </summary>
    /// <returns></returns>
    [HttpGet("info")]
    [EndpointName(EndpointName.Category.GetCategoryInfo)]
    public async Task<IResult> GetCategoryInfo()
    {
        var result = await _sender.Send(new GetCategoryInfoQuery());
        return result.Match(Results.Ok, CustomResults.Problem);
    }

}