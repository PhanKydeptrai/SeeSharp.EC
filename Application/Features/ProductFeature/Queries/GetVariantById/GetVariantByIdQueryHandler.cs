using Application.Abstractions.Messaging;
using Application.DTOs.Product;
using Application.IServices;
using Domain.Entities.ProductVariants;
using Domain.Utilities.Errors;
using SharedKernel;

namespace Application.Features.ProductFeature.Queries.GetVariantById;

internal sealed class GetVariantByIdQueryHandler : IQueryHandler<GetVariantByIdQuery, ProductVariantResponse>
{
    private readonly IProductQueryServices _productQueryServices;

    public GetVariantByIdQueryHandler(IProductQueryServices productQueryServices)
    {
        _productQueryServices = productQueryServices;
    }

    public async Task<Result<ProductVariantResponse>> Handle(GetVariantByIdQuery request, CancellationToken cancellationToken)
    {
        var productVariantId = ProductVariantId.FromGuid(request.ProductVariantId);

        var reuslt = await _productQueryServices.GetVariantById(
            productVariantId, 
            cancellationToken);

        if (reuslt is null)
        {
            return Result.Failure<ProductVariantResponse>(ProductError.VariantNotFound(productVariantId));
        }
        
        return Result.Success(reuslt);
    }
}
