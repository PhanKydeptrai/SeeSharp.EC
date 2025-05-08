using Application.Abstractions.Messaging;
using Application.DTOs.Category;
using Application.IServices;
using SharedKernel;

namespace Application.Features.CategoryFeature.Queries.GetCategoryInfo;

internal sealed class GetCategoryInfoQueryHandler : IQueryHandler<GetCategoryInfoQuery, List<CategoryInfo>>
{
    private readonly ICategoryQueryServices _categoryQueryServices;
    public GetCategoryInfoQueryHandler(ICategoryQueryServices categoryQueryServices)
    {
        _categoryQueryServices = categoryQueryServices;
    }

    public async Task<Result<List<CategoryInfo>>> Handle(GetCategoryInfoQuery request, CancellationToken cancellationToken)
    {
        var categoryInfo = await _categoryQueryServices.GetCategoryInfo();
        return Result.Success(categoryInfo);
    }
}
