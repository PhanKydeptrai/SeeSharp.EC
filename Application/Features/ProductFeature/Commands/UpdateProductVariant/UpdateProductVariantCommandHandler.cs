using Application.Abstractions.Messaging;
using Domain.Entities.ProductVariants;
using Domain.IRepositories;
using Domain.IRepositories.Products;
using Domain.Utilities.Errors;
using SharedKernel;

namespace Application.Features.ProductFeature.Commands.UpdateProductVariant;

internal sealed class UpdateProductVariantCommandHandler : ICommandHandler<UpdateProductVariantCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateProductVariantCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateProductVariantCommand request, CancellationToken cancellationToken)
    {
        var productVariantId = ProductVariantId.FromGuid(request.ProductVariantId);
        var productVariant = await _productRepository.GetProductVariant(productVariantId);
        if (productVariant is null)
        {
            return Result.Failure(ProductError.VariantNotFound(productVariantId));
        }

        //Kiểm tra khi người dùng cập nhật variant gốc của sản phẩm
        if (productVariant.IsBaseVariant == IsBaseVariant.True && request.IsBaseVariant == false)
        {
            return Result.Failure(ProductError.CannotUpdateBaseVariant(productVariantId));
        }

        if (productVariant.IsBaseVariant == IsBaseVariant.False && request.IsBaseVariant == true)
        {
            var baseVariant = await _productRepository.GetBaseVariantOfProduct(productVariant.ProductId);
            baseVariant.IsNotBase();
            
            productVariant.Update(
                VariantName.FromString(request.VariantName),
                ProductVariantPrice.FromDecimal(request.ProductVariantPrice),
                ColorCode.FromString(request.ColorCode),
                ProductVariantDescription.FromString(request.Description),
                request.ImageUrl,
                IsBaseVariant.FromBoolean(request.IsBaseVariant));

            await _unitOfWork.SaveChangeAsync();
        }

        productVariant.Update(
            VariantName.FromString(request.VariantName),
            ProductVariantPrice.FromDecimal(request.ProductVariantPrice),
            ColorCode.FromString(request.ColorCode),
            ProductVariantDescription.FromString(request.Description),
            request.ImageUrl,
            IsBaseVariant.FromBoolean(request.IsBaseVariant));

        await _unitOfWork.SaveChangeAsync();

        return Result.Success();
    }
}
