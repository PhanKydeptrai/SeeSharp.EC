using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Application.Outbox;
using Domain.Entities.Categories;
using Domain.Entities.Products;
using Domain.IRepositories;
using Domain.IRepositories.Products;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Errors;
using Domain.Utilities.Events.ProductEvents;
using SharedKernel;

namespace Application.Features.ProductFeature.Commands.UpdateProduct;

internal sealed class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOutBoxMessageServices _outboxService;
    private readonly IEventBus _eventBus;
    public UpdateProductCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        IOutBoxMessageServices outboxService,
        IEventBus eventBus)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _outboxService = outboxService;
        _eventBus = eventBus;
    }
    //FLOW: Get product by id -> Update product -> Add Outbox message -> Commit -> Publish event
    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var productId = ProductId.FromGuid(request.ProductId);
        var (product, failure) = await GetProductById(productId);
        if(product is null) return failure!;
        UpdateProduct(product, request);

        var message = CreateProductUpdatedEvent(product);
        await OutboxMessageExtentions.InsertOutboxMessageAsync(
            message.MessageId,
            message,
            _outboxService);
        await _unitOfWork.SaveToMySQL();
        
        await _eventBus.PublishAsync(message);

        return Result.Success();
    }

    //Private methods
    private ProductUpdatedEvent CreateProductUpdatedEvent(Product product)
    {
        return new ProductUpdatedEvent(
            product.ProductId.Value,
            product.ProductName.Value,
            product.ImageUrl ?? string.Empty,
            product.Description ?? string.Empty,
            product.ProductPrice.Value,
            product.ProductStatus,
            product.CategoryId.Value,
            Ulid.NewUlid().ToGuid());
    }
    private async Task<(Product? product, Result? result)> GetProductById(ProductId productId)
    {
        var product = await _productRepository.GetProductFromMySQL(productId);
        if(product is null) return (null, Result.Failure(ProductError.NotFound(productId)));
        return (product, null);
    }
    private void UpdateProduct(Product product, UpdateProductCommand request)
    {
        product.Update(
            ProductName.NewProductName(request.ProductName),
            string.Empty,
            request.Description,
            ProductPrice.NewProductPrice(request.ProductPrice),
            product.ProductStatus,
            CategoryId.FromGuid(request.CategoryId));
    }
}
