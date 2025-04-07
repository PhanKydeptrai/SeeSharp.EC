using Application.Abstractions.Messaging;
using Application.DTOs.Product;
using Application.Features.Pages;
using Application.IServices;
using SharedKernel;

namespace Application.Features.ProductFeature.Queries.GetAllVariantQuery;

internal sealed class GetAllVariantQueryHandler 
    : IQueryHandler<GetAllVariantQuery, PagedList<ProductVariantResponse>>
{
    private readonly IProductQueryServices _productQueryServices;

    public GetAllVariantQueryHandler(IProductQueryServices productQueryServices)
    {
        _productQueryServices = productQueryServices;
    }

    public async Task<Result<PagedList<ProductVariantResponse>>> Handle(
        GetAllVariantQuery request, 
        CancellationToken cancellationToken)
    {
        var pagedList = await _productQueryServices.GetAllVariant(
            request.filterProductStatus,
            request.filterProduct,
            request.filterCategory,
            request.searchTerm,
            request.sortColumn,
            request.sortOrder,
            request.page,
            request.pageSize);

        return Result.Success(pagedList);
    }
}
