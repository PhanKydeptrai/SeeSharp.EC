using Application.Abstractions.Messaging;
using Application.Services;
using Domain.Entities.ProductVariants;
using Domain.IRepositories;
using Domain.IRepositories.Products;
using Domain.Utilities.Errors;
using SharedKernel;

namespace Application.Features.ProductFeature.Commands.UpdateProductVariant;

internal sealed class UpdateProductVariantCommandHandler : ICommandHandler<UpdateProductVariantCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly CloudinaryService _cloudinaryService;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateProductVariantCommandHandler(
        IProductRepository productRepository, 
        IUnitOfWork unitOfWork, 
        CloudinaryService cloudinaryService)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _cloudinaryService = cloudinaryService;
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
        
        
        if (request.Image is not null)
        {
            string oldimageUrl = productVariant.ImageUrl!;
            //Xử lý lưu ảnh mới
            string newImageUrl = string.Empty;
            //Upload ảnh lên cloudinary
            newImageUrl = await _cloudinaryService.UploadNewImage(request.Image);


            if (productVariant.IsBaseVariant == IsBaseVariant.False && request.IsBaseVariant == true)
            {
                var baseVariant = await _productRepository.GetBaseVariantOfProduct(productVariant.ProductId);
                baseVariant.IsNotBase();

                productVariant.Update(
                    VariantName.FromString(request.VariantName),
                    ProductVariantPrice.FromDecimal(request.ProductVariantPrice),
                    ColorCode.FromString(request.ColorCode),
                    ProductVariantDescription.FromString(request.Description),
                    newImageUrl, 
                    IsBaseVariant.FromBoolean(request.IsBaseVariant));

                await _unitOfWork.SaveChangesAsync();
                return Result.Success();
            }

            productVariant.Update(
                VariantName.FromString(request.VariantName),
                ProductVariantPrice.FromDecimal(request.ProductVariantPrice),
                ColorCode.FromString(request.ColorCode),
                ProductVariantDescription.FromString(request.Description),
                newImageUrl, 
                IsBaseVariant.FromBoolean(request.IsBaseVariant));

            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
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
                productVariant.ImageUrl, 
                IsBaseVariant.FromBoolean(request.IsBaseVariant));

            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        productVariant.Update(
            VariantName.FromString(request.VariantName),
            ProductVariantPrice.FromDecimal(request.ProductVariantPrice),
            ColorCode.FromString(request.ColorCode),
            ProductVariantDescription.FromString(request.Description),
            productVariant.ImageUrl, 
            IsBaseVariant.FromBoolean(request.IsBaseVariant));

        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
