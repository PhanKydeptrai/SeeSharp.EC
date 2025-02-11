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

    public GetCategoryByIdQueryHandler(ICategoryQueryServices categoryQueryServices)
    {
        _categoryQueryServices = categoryQueryServices;
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

        return Result.Success(category);
    }
}
