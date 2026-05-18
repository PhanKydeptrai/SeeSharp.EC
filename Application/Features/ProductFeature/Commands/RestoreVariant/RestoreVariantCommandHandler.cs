using Application.Abstractions.Messaging;
using Domain.Entities.ProductVariants;
using Domain.Events.ProductVariantEvents;
using Domain.IRepositories;
using Domain.IRepositories.Products;
using Domain.Utilities.Errors;
using MediatR;
using SharedKernel;

namespace Application.Features.ProductFeature.Commands.RestoreVariant;

internal sealed class RestoreVariantCommandHandler : ICommandHandler<RestoreVariantCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IPublisher _publisher;
    private readonly IUnitOfWork _unitOfWork;

    public RestoreVariantCommandHandler(
        IProductRepository productRepository, 
        IPublisher publisher,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _publisher = publisher;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(RestoreVariantCommand request, CancellationToken cancellationToken)
    {
        var variantId = ProductVariantId.FromGuid(request.VariantId);
        var variant = await _productRepository.GetProductVariant(variantId);

        if (variant is null)
        {
            return Result.Failure(ProductError.VariantNotFound(variantId));
        }

        if (variant.ProductVariantStatus != ProductVariantStatus.Discontinued)
        {
            return Result.Failure(ProductError.VariantNotDiscontinued(variantId));
        }

        variant.Restore();

        await _unitOfWork.SaveChangesAsync();

        await _publisher.Publish(new ProductVariantRestoredEvent(variant.ProductVariantId, variant.ProductId, variant.Product!.CategoryId), cancellationToken);

        return Result.Success();
    }
}
