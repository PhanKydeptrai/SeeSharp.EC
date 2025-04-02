using Application.Abstractions.Messaging;
using Domain.Entities.Categories;
using Domain.Entities.Products;
using Domain.IRepositories;
using Domain.IRepositories.Products;
using Domain.IRepositories.CategoryRepositories;
using Domain.Utilities.Errors;
using SharedKernel;
using Domain.Entities.ProductVariants;

namespace Application.Features.ProductFeature.Commands.CreateProduct;

internal sealed class CreateProductCommandHandler : ICommandHandler<CreateProductCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    public CreateProductCommandHandler(
        IUnitOfWork unitOfWork,
        IProductRepository productRepository,
        ICategoryRepository categoryRepository)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }
    
    public async Task<Result> Handle(
        CreateProductCommand request, 
        CancellationToken cancellationToken)
    {
        var (product, productVariant, failure) = await CreateNewProduct(request);

        if (product is null) return failure!;
        
        await _productRepository.AddProductToPostgreSQL(product);
        await _productRepository.AddProductVariantToPostgreSQL(productVariant!);
        
        await _unitOfWork.SaveChangeAsync();
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

        //TODO: Xử lý ảnh
        //--------------------


        //--------------------
        
        var newProduct = Product.NewProduct(
            ProductName.NewProductName(command.ProductName),
            string.Empty, //TODO: Xử lý ảnh
            command.Description ?? string.Empty,
            categoryId);

        var newProductVariant = ProductVariant.Create(
            VariantName.FromString(command.ProductBaseVariantName),
            ProductVariantPrice.FromDecimal(command.VariantPrice),
            ColorCode.Create(command.ColorCode),
            ProductVariantDescription.FromString(command.Description ?? string.Empty),
            newProduct.ProductId,
            string.Empty, //TODO: Xử lý ảnh
            IsBaseVariant.True);
        
        return (newProduct, newProductVariant, null);
    }
    #endregion ----------------------------------------------------
}
