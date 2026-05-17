using Application.Abstractions.Messaging;
using Domain.Entities.Products;
using Domain.Events.ProductEvents;
using Domain.IRepositories;
using Domain.IRepositories.Products;
using Domain.Utilities.Errors;
using MediatR;
using SharedKernel;

namespace Application.Features.ProductFeature.Commands.RestoreProduct;

internal sealed class RestoreProductCommandHandler : ICommandHandler<RestoreProductCommand>
{
    #region Dependency Injection
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;
    private readonly IPublisher _publisher;
    public RestoreProductCommandHandler(
        IUnitOfWork unitOfWork,
        IProductRepository productRepository,
        IPublisher publisher)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _publisher = publisher;
    }
    #endregion

    public async Task<Result> Handle(RestoreProductCommand request, CancellationToken cancellationToken)
    {
        //Start transaction
        using var transaction = await _unitOfWork.BeginPostgreSQLTransaction();
        var productId = ProductId.FromGuid(request.ProductId);
        var (product, failure) = await GetProduct(productId);
        if (product is null)
        {
            transaction.Rollback();
            return failure!;
        }

        product.Restore();
        await _unitOfWork.SaveChangesAsync();
        await _productRepository.RestoreProductVariantByProduct(productId);
        transaction.Commit();
        await _publisher.Publish(new ProductRestoredEvent(productId), CancellationToken.None);
        return Result.Success();
    }

    private async Task<(Product? product, Result? failure)> GetProduct(ProductId productId)
    {
        var product = await _productRepository.GetProduct(productId);
        if (product is null)
        {
            return (null, Result.Failure(ProductError.ProductNotFound(productId)));
        }
        if (product.ProductStatus != ProductStatus.Discontinued)
        {
            return (null, Result.Failure(ProductError.ProductNotDiscontinued(productId)));
        }

        return (product, null);
    }
}
