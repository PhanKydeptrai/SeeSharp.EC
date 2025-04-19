using Application.Abstractions.Messaging;
using Application.Services;
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
    private readonly CloudinaryService _cloudinaryService;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateProductCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        CloudinaryService cloudinaryService)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _cloudinaryService = cloudinaryService;
    }
    #endregion
    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var productId = ProductId.FromGuid(request.ProductId);
        var (product, failure) = await GetProductById(productId);
        if (product is null) return failure!;

        if (request.ProductImage is not null)
        {
            string oldimageUrl = product.ImageUrl!;

            //Xử lý lưu ảnh mới
            string newImageUrl = string.Empty;
            // if (request.ProductImage != null)
            // {
                //tạo memory stream từ file ảnh
                var memoryStream = new MemoryStream();
                await request.ProductImage.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                //Upload ảnh lên cloudinary
                var resultUpload = await _cloudinaryService.UploadAsync(memoryStream, request.ProductImage.FileName);
                newImageUrl = resultUpload.SecureUrl.ToString(); //Nhận url ảnh từ cloudinary

                //Log                                              
                Console.WriteLine(resultUpload.JsonObj);

                UpdateProduct(product, request, newImageUrl);

                //Xóa ảnh cũ
                if (oldimageUrl != "")
                {
                    //Upload ảnh lên cloudinary
                    var resultDelete = await _cloudinaryService.DeleteAsync(oldimageUrl);
                    //Log
                    Console.WriteLine(resultDelete.JsonObj);
                }

                var baseVariant = product.ProductVariants!.FirstOrDefault(a => a.IsBaseVariant == IsBaseVariant.True);

                baseVariant!.Update(
                    baseVariant.VariantName,
                    ProductVariantPrice.FromDecimal(request.ProductPrice),
                    ColorCode.FromString(request.ColorCode),
                    ProductVariantDescription.FromString(request.Description),
                    newImageUrl,
                    IsBaseVariant.True);

                await _unitOfWork.SaveChangesAsync();
                return Result.Success();
            // }


        }
        else
        {
            UpdateProduct(product, request, product.ImageUrl!);

            var baseVariant = product.ProductVariants!.FirstOrDefault(a => a.IsBaseVariant == IsBaseVariant.True);

            baseVariant!.Update(
                baseVariant.VariantName,
                ProductVariantPrice.FromDecimal(request.ProductPrice),
                ColorCode.FromString(request.ColorCode),
                ProductVariantDescription.FromString(request.Description),
                product.ImageUrl!,
                IsBaseVariant.True);

            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }

    #region Private method

    private async Task<(Product? product, Result? result)> GetProductById(ProductId productId)
    {
        var product = await _productRepository.GetProduct(productId);
        if (product is null) return (null, Result.Failure(ProductError.ProductNotFound(productId)));
        return (product, null);
    }

    private void UpdateProduct(Product product, UpdateProductCommand request, string imageUrl)
    {
        var categoryId = CategoryId.DefaultCategoryId;

        if (request.CategoryId is not null)
        {
            categoryId = CategoryId.FromGuid(request.CategoryId.Value);
        }

        product.Update(
            ProductName.NewProductName(request.ProductName),
            imageUrl,
            request.Description,
            product.ProductStatus,
            categoryId);
    }
    #endregion
}
