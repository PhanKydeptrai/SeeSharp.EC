using Application.Abstractions.LinkService;
using Application.Abstractions.Messaging;
using Application.DTOs.Category;
using Application.Features.Pages;
using Application.IServices;
using SharedKernel;

namespace Application.Features.CategoryFeature.Queries.GetAllCategory;

internal sealed class GetAllCategoryQueryHandler : IQueryHandler<GetAllCategoryQuery, PagedList<CategoryResponse>>
{
    private readonly ICategoryQueryServices _categoryQueryServices;
    private readonly ILinkServices _linkService;
    public GetAllCategoryQueryHandler(
        ICategoryQueryServices categoryQueryServices, 
        ILinkServices linkService)
    {
        _categoryQueryServices = categoryQueryServices;
        _linkService = linkService;
    }

    public async Task<Result<PagedList<CategoryResponse>>> Handle(
        GetAllCategoryQuery request, 
        CancellationToken cancellationToken)
    {
        var pagedList = await _categoryQueryServices.PagedList(
            request.filter,
            request.searchTerm,
            request.sortColumn,
            request.sortOrder,
            request.page,
            request.pageSize
        );
        AddLinks(request, pagedList);

        return Result.Success(pagedList);
    }

    private void AddLinks(GetAllCategoryQuery request, PagedList<CategoryResponse> pagedList)
    {
        pagedList.Links.Add(_linkService.Generate(
            "GetAllCategory",
            new
            {
                filter = request.filter,
                searchTerm = request.searchTerm,
                sortColumn = request.sortColumn,
                sortOrder = request.sortOrder,
                page = request.page,
                pageSize = request.pageSize

            },
            "self",
            "GET"
        ));

        if (pagedList.HaspreviousPage)
        {
            pagedList.Links.Add(_linkService.Generate(
                "GetAllCategory",
                new
                {
                    filter = request.filter,
                    searchTerm = request.searchTerm,
                    sortColumn = request.sortColumn,
                    sortOrder = request.sortOrder,
                    page = request.page - 1,
                    pageSize = request.pageSize
                },
                "previous-page",
                "GET"
            ));
        }

        if (pagedList.HasNextPage)
        {
            pagedList.Links.Add(_linkService.Generate(
                "GetAllCategory",
                new
                {
                    filter = request.filter,
                    searchTerm = request.searchTerm,
                    sortColumn = request.sortColumn,
                    sortOrder = request.sortOrder,
                    page = request.page + 1,
                    pageSize = request.pageSize
                },
                "next-page",
                "GET"
            ));
        }
    }
}
