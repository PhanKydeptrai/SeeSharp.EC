using Application.Abstractions.LinkService;
using Application.Abstractions.Messaging;
using Application.DTOs.Category;
using Application.IServices;
using Domain.Entities.Categories;
using Domain.Utilities.Errors;
using SharedKernel;
using SharedKernel.Constants;

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
            CategoryId.FromGuid(request.categoryId),
            cancellationToken);

        if (category is null)
        {
            return Result.Failure<CategoryResponse>(
                CategoryErrors.NotFound(
                    CategoryId.FromGuid(
                        request.categoryId)));
        }

        AddLinkForCategory(category);

        return Result.Success(category);
    }

    private void AddLinkForCategory(CategoryResponse categoryResponse)
    {
        categoryResponse.links.Add(_linkServices.Generate(
            EndpointName.Category.GetById,
            new { categoryId = categoryResponse.categoryId },
            "self",
            EndpointMethod.GET));

        categoryResponse.links.Add(_linkServices.Generate(
            EndpointName.Category.Update,
            new { categoryId = categoryResponse.categoryId },
            "update-category",
            EndpointMethod.PUT));

        if (categoryResponse.categoryStatus == CategoryStatus.Deleted.ToString())
        {
            categoryResponse.links.Add(_linkServices.Generate(
                        EndpointName.Category.Restore,
                        new { categoryId = categoryResponse.categoryId },
                        "restore-category",
                        EndpointMethod.PUT));
        }

        if (categoryResponse.categoryStatus == CategoryStatus.Available.ToString())
        {
            categoryResponse.links.Add(_linkServices.Generate(
                        EndpointName.Category.Delete,
                        new { categoryId = categoryResponse.categoryId },
                        "delete-category",
                        EndpointMethod.DELETE));
        }

    }
}
