using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Application.Outbox;
using Domain.Entities.Products;
using Domain.IRepositories;
using Domain.IRepositories.Categories;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Errors;
using Domain.Utilities.Events.ProductEvents;
using SharedKernel;

namespace Application.Features.ProductFeature.Commands.RestoreProduct;

internal sealed class RestoreProductCommandHandler : ICommandHandler<RestoreProductCommand>
{
    #region Dependency Injection
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;
    private readonly IOutBoxMessageServices _outboxService;
    private readonly IEventBus _eventBus;
    public RestoreProductCommandHandler(
        IUnitOfWork unitOfWork,
        IProductRepository productRepository,
        IOutBoxMessageServices outboxService,
        IEventBus eventBus)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _outboxService = outboxService;
        _eventBus = eventBus;
    }
    #endregion

    public async Task<Result> Handle(RestoreProductCommand request, CancellationToken cancellationToken)
    {
        var productId = ProductId.FromGuid(request.ProductId);
        var (product, failure) = await GetProduct(productId);
        if (product is null) return failure!;
        product.Restore();
        var message = new ProductRestoredEvent(product.ProductId.Value, Ulid.NewUlid().ToGuid());
        await OutboxMessageExtentions.InsertOutboxMessageAsync(message.MessageId, message, _outboxService);
        await _unitOfWork.SaveToMySQL();

        //Publish event
        await _eventBus.PublishAsync(message);

        return Result.Success();
    }

    private async Task<(Product? product, Result? failure)> GetProduct(ProductId productId)
    {
        var product = await _productRepository.GetProductFromMySQL(productId);
        if (product is null)
        {
            return (null, Result.Failure(ProductError.NotFound(productId)));
        }
        if (product.ProductStatus != ProductStatus.Discontinued)
        {
            return (null, Result.Failure(ProductError.NotDiscontinued(productId)));
        }

        return (product, null);
    }
}
