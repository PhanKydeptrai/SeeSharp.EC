using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Application.Outbox;
using Domain.Entities.Categories;
using Domain.Entities.Products;
using Domain.IRepositories;
using Domain.IRepositories.Categories;
using Domain.IRepositories.CategoryRepositories;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Errors;
using Domain.Utilities.Events.ProductEvents;
using SharedKernel;

namespace Application.Features.ProductFeature.Commands.CreateProduct;

internal sealed class CreateProductCommandHandler : ICommandHandler<CreateProductCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;
    private readonly IEventBus _eventBus;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    public CreateProductCommandHandler(
        IUnitOfWork unitOfWork,
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IOutBoxMessageServices outBoxMessageServices,
        IEventBus eventBus)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _outBoxMessageServices = outBoxMessageServices;
        _eventBus = eventBus;
    }
    //FLOW: Add new product to the database -> Add Outbox message -> Commit -> Publish event
    public async Task<Result> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var (product, failure) = await CreateNewProduct(request);

        if (product is null) return failure!;

        await _productRepository.AddProductToMySQL(product);

        var message = CreateProductCreatedEvent(product);
        await OutboxMessageExtentions.InsertOutboxMessageAsync(
            message.MessageId,
            message,
            _outBoxMessageServices);

        int result = await _unitOfWork.SaveToMySQL();

        if (result > 0)
        {
            await _eventBus.PublishAsync(message, cancellationToken);
            return Result.Success(product.ProductId);
        }
        return Result.Failure(ProductError.Failure(product.ProductId));
    }



    #region Private Method
    private ProductCreatedEvent CreateProductCreatedEvent(Product product)
    {
        return new ProductCreatedEvent(
            product.ProductId.Value,
            product.ProductName.Value,
            product.ImageUrl,
            product.Description,
            product.ProductPrice.Value,
            product.ProductStatus,
            product.CategoryId.Value,
            Ulid.NewUlid().ToGuid());
    }
    private async Task<(Product? category, Result? failure)> CreateNewProduct(
        CreateProductCommand command,
        CancellationToken cancellationToken = default)
    {
        var categoryId = CategoryId.FromString(command.CategoryId);
        if (!await _categoryRepository.IsCategoryIdExist(categoryId, cancellationToken))
        {
            return (null, Result.Failure(CategoryErrors.NotFound(categoryId)));
        }

        //TODO: Xử lý ảnh
        //--------------------


        //--------------------

        return (Product.NewProduct(
            ProductName.FromString(command.ProductName),
            string.Empty, //TODO: Xử lý ảnh
            command.Description ?? string.Empty,
            ProductPrice.FromDecimal(command.Price),
            categoryId), null);
    }
    #endregion ----------------------------------------------------
}
