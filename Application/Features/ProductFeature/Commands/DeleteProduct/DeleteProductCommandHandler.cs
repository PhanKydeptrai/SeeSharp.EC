using Application.Abstractions.Messaging;
using Domain.Entities.Products;
using Domain.IRepositories;
using Domain.IRepositories.Products;
using Domain.Utilities.Errors;
using SharedKernel;

namespace Application.Features.ProductFeature.Commands.DeleteProduct;

internal sealed class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    public DeleteProductCommandHandler(
        IUnitOfWork unitOfWork,
        IProductRepository productRepository)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
    }
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
        var product = await _productRepository.GetProduct(productId);
        if (product is null || product.ProductStatus == ProductStatus.Discontinued)
        {
            return (null, Result.Failure(ProductError.VariantNotFound(productId)));
        }
        return (product, null);
    }


}

