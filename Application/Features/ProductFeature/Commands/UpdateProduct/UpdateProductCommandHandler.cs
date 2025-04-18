using Application.Abstractions.Messaging;
using Domain.Entities.Categories;
using Domain.Entities.Products;
using Domain.Entities.ProductVariants;
using Domain.IRepositories;
using Domain.IRepositories.Products;
using Domain.Utilities.Errors;
using SharedKernel;

namespace Application.Features.ProductFeature.Commands.UpdateProduct;

internal sealed class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
{
    #region Dependencies
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateProductCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }
    #endregion
    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var productId = ProductId.FromGuid(request.ProductId);
        var (product, failure) = await GetProductById(productId);
        if (product is null) return failure!;
        UpdateProduct(product, request);

        var baseVariant = product.ProductVariants!.FirstOrDefault(a => a.IsBaseVariant == IsBaseVariant.True);
        
        baseVariant!.Update(
            baseVariant.VariantName,
            ProductVariantPrice.FromDecimal(request.ProductPrice),
            ColorCode.FromString(request.ColorCode),
            ProductVariantDescription.FromString(request.Description),
            string.Empty, //TODO: Update ImageUrl
            IsBaseVariant.True);

        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    #region Private method

    private async Task<(Product? product, Result? result)> GetProductById(ProductId productId)
    {
        var product = await _productRepository.GetProduct(productId);
        if (product is null) return (null, Result.Failure(ProductError.ProductNotFound(productId)));
        return (product, null);
    }

    private void UpdateProduct(Product product, UpdateProductCommand request)
    {
        var categoryId = CategoryId.DefaultCategoryId;

        if (request.CategoryId is not null)
        {
            categoryId = CategoryId.FromGuid(request.CategoryId.Value);
        }

        product.Update(
            ProductName.NewProductName(request.ProductName),
            string.Empty, //TODO: Update ImageUrl
            request.Description,
            product.ProductStatus,
            categoryId);
    }
    #endregion
}
