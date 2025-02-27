using Application.Abstractions.LinkService;
using Application.Abstractions.Messaging;
using Application.DTOs.Category;
using Application.Features.Pages;
using Application.IServices;
using Domain.Entities.Categories;
using SharedKernel;
using SharedKernel.Constants;

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

        foreach (var category in pagedList.Items)
        {
            AddLinkForCategory(category);
        }

        AddLinks(request, pagedList);

        return Result.Success(pagedList);
    }

    private void AddLinks(GetAllCategoryQuery request, PagedList<CategoryResponse> pagedList)
    {
        pagedList.Links.Add(_linkService.Generate(
            EndpointName.Category.GetAll,
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
            EndpointMethod.GET
        ));

        if (pagedList.HaspreviousPage)
        {
            pagedList.Links.Add(_linkService.Generate(
                EndpointName.Category.GetAll,
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
                EndpointMethod.GET
            ));
        }

        if (pagedList.HasNextPage)
        {
            pagedList.Links.Add(_linkService.Generate(
                EndpointName.Category.GetAll,
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
                EndpointMethod.GET
            ));
        }
    }

    private void AddLinkForCategory(CategoryResponse categoryResponse)
    {
        categoryResponse.links.Add(_linkService.Generate(
            EndpointName.Category.GetById,
            new { categoryId = categoryResponse.categoryId },
            "self",
            EndpointMethod.GET));

        categoryResponse.links.Add(_linkService.Generate(
            EndpointName.Category.Update,
            new { categoryId = categoryResponse.categoryId },
            "update-category",
            EndpointMethod.PUT));

        if (categoryResponse.categoryStatus == CategoryStatus.Deleted.ToString())
        {
            categoryResponse.links.Add(_linkService.Generate(
                        EndpointName.Category.Restore,
                        new { categoryId = categoryResponse.categoryId },
                        "restore-category",
                        EndpointMethod.PUT));
        }

        if (categoryResponse.categoryStatus == CategoryStatus.Available.ToString())
        {
            categoryResponse.links.Add(_linkService.Generate(
                        EndpointName.Category.Delete,
                        new { categoryId = categoryResponse.categoryId },
                        "delete-category",
                        EndpointMethod.DELETE));
        }

    }
}
