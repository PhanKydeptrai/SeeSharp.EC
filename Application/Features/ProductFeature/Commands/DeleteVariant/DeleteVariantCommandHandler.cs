using Application.Abstractions.Messaging;
using Domain.Entities.ProductVariants;
using Domain.IRepositories;
using Domain.IRepositories.Products;
using Domain.Utilities.Errors;
using SharedKernel;

namespace Application.Features.ProductFeature.Commands.DeleteVariant;

internal sealed class DeleteVariantCommandHandler : ICommandHandler<DeleteVariantCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;

    public DeleteVariantCommandHandler(
        IUnitOfWork unitOfWork, 
        IProductRepository productRepository)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
    }

    public async Task<Result> Handle(DeleteVariantCommand request, CancellationToken cancellationToken)
    {
        var variantId = ProductVariantId.FromGuid(request.VariantId);
        var productVariant = await _productRepository.GetProductVariant(variantId);

        if(productVariant is null)
        {
            return Result.Failure(ProductError.VariantNotFound(variantId));
        }

        productVariant.Delete();
        await _unitOfWork.SaveChangeAsync();

        return Result.Success();
    }
}
