using Application.Abstractions.Messaging;
using Domain.Entities.Categories;
using Domain.Entities.Products;
using Domain.IRepositories;
using Domain.IRepositories.Products;
using Domain.IRepositories.CategoryRepositories;
using Domain.Utilities.Errors;
using SharedKernel;
using Domain.Entities.ProductVariants;
using CloudinaryDotNet;
using Application.Services;

namespace Application.Features.ProductFeature.Commands.CreateProduct;

internal sealed class CreateProductCommandHandler : ICommandHandler<CreateProductCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly CloudinaryService _cloudinaryService;
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    public CreateProductCommandHandler(
        IUnitOfWork unitOfWork,
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        CloudinaryService cloudinaryService)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _cloudinaryService = cloudinaryService;
    }

    public async Task<Result> Handle(
        CreateProductCommand request, 
        CancellationToken cancellationToken)
    {
        var (product, productVariant, failure) = await CreateNewProduct(request);

        if (product is null) return failure!;
        
        await _productRepository.AddProduct(product);
        await _productRepository.AddProductVariant(productVariant!);
        
        await _unitOfWork.SaveChangesAsync();
        return Result.Success(product.ProductId);
    }



    #region Private Method
    private async Task<(Product? product, ProductVariant? productVariant, Result? failure)> CreateNewProduct(
        CreateProductCommand command,
        CancellationToken cancellationToken = default)
    {
        CategoryId categoryId = CategoryId.DefaultCategoryId;
        
        if (command.CategoryId is not null)
        {
            categoryId = CategoryId.FromGuid(command.CategoryId.Value);
        }

        if (!await _categoryRepository.IsCategoryIdExist(categoryId, cancellationToken))
        {
            return (null, null, Result.Failure(CategoryErrors.NotFound(categoryId)));
        }

        
        //Xử lý lưu 
        string imageUrl = string.Empty;
        if (command.ProductImage != null)
        {

            //tạo memory stream từ file ảnh
            var memoryStream = new MemoryStream();
            await command.ProductImage.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            //Upload ảnh lên cloudinary
            
            var resultUpload = await _cloudinaryService.UploadAsync(memoryStream, command.ProductImage.FileName);
            imageUrl = resultUpload.SecureUrl.ToString(); //Nhận url ảnh từ cloudinary
            //Log
            Console.WriteLine(resultUpload.JsonObj);
        }

        //--------------------
        
        var newProduct = Product.NewProduct(
            ProductName.NewProductName(command.ProductName),
            imageUrl,
            command.Description ?? string.Empty,
            categoryId);

        var newProductVariant = ProductVariant.Create(
            VariantName.FromString(command.ProductBaseVariantName),
            ProductVariantPrice.FromDecimal(command.VariantPrice),
            ColorCode.Create(command.ColorCode),
            ProductVariantDescription.FromString(command.Description ?? string.Empty),
            newProduct.ProductId,
            imageUrl,
            IsBaseVariant.True);
        
        return (newProduct, newProductVariant, null);
    }
    #endregion ----------------------------------------------------
}
