using Application.Abstractions.LinkService;
using Application.Abstractions.Messaging;
using Application.DTOs.Product;
using Application.IServices;
using Domain.Entities.Products;
using Domain.Utilities.Errors;
using SharedKernel;
using SharedKernel.Constants;

namespace Application.Features.ProductFeature.Queries.GetProductById;

public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductResponse>
{
    private readonly ILinkServices _linkServices;
    private readonly IProductQueryServices _productQueryServices;
    public GetProductByIdQueryHandler(
        ILinkServices linkServices, 
        IProductQueryServices productQueryServices)
    {
        _linkServices = linkServices;
        _productQueryServices = productQueryServices;
    }

    public async Task<Result<ProductResponse>> Handle(
        GetProductByIdQuery request, 
        CancellationToken cancellationToken)
    {
        var product = await _productQueryServices.GetById(
            ProductId.FromGuid(request.productId),
            cancellationToken);

        if (product is null)
        {
            return Result.Failure<ProductResponse>(
                ProductError.NotFound(
                    ProductId.FromGuid(
                        request.productId)));
        }

        AddLinkForProduct(product);
        return Result.Success(product);

    }

    private void AddLinkForProduct(ProductResponse productResponse)
    {
        productResponse.links.Add(_linkServices.Generate(
            EndpointName.Product.GetById,
            new { productId = productResponse.ProductId },
            "self",
            "GET"));
        productResponse.links.Add(_linkServices.Generate(
            EndpointName.Product.Update,
            new { productId = productResponse.ProductId },
            "update-product",
            "PUT"));
        productResponse.links.Add(_linkServices.Generate(
            EndpointName.Product.Delete,
            new { productId = productResponse.ProductId },
            "delete-product",
            "DELETE"));
    }
}