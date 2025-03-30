using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Domain.Entities.Products;
using Domain.IRepositories;
using Domain.IRepositories.Products;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Errors;
using SharedKernel;

namespace Application.Features.ProductFeature.Commands.DeleteProduct;

internal sealed class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IOutBoxMessageServices _outboxService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventBus _eventBus;
    public DeleteProductCommandHandler(
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
    //FLOW: Get product by id -> Delete product -> Create outbox message -> Commit -> Publish event
    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var productId = ProductId.FromGuid(request.ProductId);
        var (product, failure) = await GetProductByIdAsync(productId);
        if (product is null) return failure!;
        product.Delete();
        await _unitOfWork.SaveChangeAsync();

        return Result.Success();
    }

    //Private methods
    private async Task<(Product? product, Result? failure)> GetProductByIdAsync(ProductId productId)
    {
        var product = await _productRepository.GetProductFromPostgreSQL(productId);
        if (product is null || product.ProductStatus == ProductStatus.Discontinued)
        {
            return (null, Result.Failure(ProductError.NotFound(productId)));
        }
        return (product, null);
    }


}

