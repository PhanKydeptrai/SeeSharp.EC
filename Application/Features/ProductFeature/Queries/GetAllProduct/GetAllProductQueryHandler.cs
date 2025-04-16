using Application.Abstractions.LinkService;
using Application.Abstractions.Messaging;
using Application.DTOs.Product;
using Application.Features.Pages;
using Application.IServices;
using SharedKernel;
using SharedKernel.Constants;

namespace Application.Features.ProductFeature.Queries.GetAllProduct;

internal sealed class GetAllProductQueryHandler : IQueryHandler<GetAllProductQuery, PagedList<ProductResponse>>
{
    private readonly IProductQueryServices _productQueryServices;
    private readonly ILinkServices _linkServices;
    public GetAllProductQueryHandler(
        IProductQueryServices productQueryServices,
        ILinkServices linkServices)
    {
        _productQueryServices = productQueryServices;
        _linkServices = linkServices;
    }

    public async Task<Result<PagedList<ProductResponse>>> Handle(
        GetAllProductQuery request,
        CancellationToken cancellationToken)
    {
        var pagedList = await _productQueryServices.GetAllProductWithVariantList(
            request.filterProductStatus,
            request.filterCategory,
            request.searchTerm,
            request.sortColumn,
            request.sortOrder,
            request.page,
            request.pageSize);

        // AddLinks(request, pagedList);
        return Result.Success(pagedList);
    }

    // private void AddLinks(GetAllProductQuery request, PagedList<ProductResponse> pagedList)
    // {
    //     pagedList.Links.Add(_linkServices.Generate(
    //         EndpointName.Product.GetAll,
    //         new
    //         {
    //             filterProductStatus = request.filterProductStatus,
    //             filterCategory = request.filterCategory,
    //             searchTerm = request.searchTerm,
    //             sortColumn = request.sortColumn,
    //             sortOrder = request.sortOrder,
    //             page = request.page,
    //             pageSize = request.pageSize
    //         },
    //         "self",
    //         EndpointMethod.GET));

    //     if (pagedList.HaspreviousPage)
    //     {
    //         pagedList.Links.Add(_linkServices.Generate(
    //             EndpointName.Product.GetAll,
    //             new
    //             {
    //                 filterProductStatus = request.filterProductStatus,
    //                 filterCategory = request.filterCategory,
    //                 searchTerm = request.searchTerm,
    //                 sortColumn = request.sortColumn,
    //                 sortOrder = request.sortOrder,
    //                 page = request.page - 1,
    //                 pageSize = request.pageSize
    //             },
    //             "previous-page",
    //             EndpointMethod.GET));
    //     }


    //     if (pagedList.HasNextPage)
    //     {
    //         pagedList.Links.Add(_linkServices.Generate(
    //             EndpointName.Product.GetAll,
    //             new
    //             {
    //                 filterProductStatus = request.filterProductStatus,
    //                 filterCategory = request.filterCategory,
    //                 searchTerm = request.searchTerm,
    //                 sortColumn = request.sortColumn,
    //                 sortOrder = request.sortOrder,
    //                 page = request.page + 1,
    //                 pageSize = request.pageSize
    //             },
    //             "next-page",
    //             EndpointMethod.GET));
    //     }


    // }
}
