using Application.Abstractions.LinkService;
using Application.Abstractions.Messaging;
using Application.DTOs.Category;
using Application.IServices;
using Domain.Entities.Categories;
using Domain.Utilities.Errors;
using SharedKernel;

namespace Application.Features.CategoryFeature.Queries.GetCategoryById;

internal sealed class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, CategoryResponse>
{
    private readonly ICategoryQueryServices _categoryQueryServices;
    private readonly ILinkServices _linkServices;
    public GetCategoryByIdQueryHandler(
        ICategoryQueryServices categoryQueryServices, 
        ILinkServices linkServices)
    {
        _categoryQueryServices = categoryQueryServices;
        _linkServices = linkServices;
    }

    public async Task<Result<CategoryResponse>> Handle(
        GetCategoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        var category = await _categoryQueryServices.GetById(
            CategoryId.FromString(request.categoryId),
            cancellationToken);

        if (category is null)
        {
            return Result.Failure<CategoryResponse>(
                CategoryErrors.NotFound(
                    CategoryId.FromString(
                        request.categoryId)));
        }

        AddLinkForCategory(category);

        return Result.Success(category);
    }

    private void AddLinkForCategory(CategoryResponse categoryResponse)
    {
        categoryResponse.links.Add(_linkServices.Generate(
            "GetCategoryById", 
            new { categoryId = categoryResponse.categoryId }, 
            "self", 
            "GET"));

        categoryResponse.links.Add(_linkServices.Generate(
            "UpdateCategory",
            new { categoryId = categoryResponse.categoryId },
            "update-category",
            "PUT"));

        categoryResponse.links.Add(_linkServices.Generate(
            "GetCategoryById",
            new { categoryId = categoryResponse.categoryId },
            "delete-category",
            "DELETE"));
    }
}
