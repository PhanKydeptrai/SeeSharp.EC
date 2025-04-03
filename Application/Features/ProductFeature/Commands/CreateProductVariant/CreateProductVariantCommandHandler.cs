using Application.Abstractions.Messaging;
using Domain.Entities.Products;
using Domain.Entities.ProductVariants;
using Domain.IRepositories;
using Domain.IRepositories.Products;
using SharedKernel;

namespace Application.Features.ProductFeature.Commands.CreateProductVariant;

internal sealed class CreateProductVariantCommandHandler : ICommandHandler<CreateProductVariantCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    public CreateProductVariantCommandHandler(
        IProductRepository productRepository, 
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        CreateProductVariantCommand request, 
        CancellationToken cancellationToken)
    {
        var productVariant = ProductVariant.Create(
            VariantName.NewVariantName(request.VariantName),
            ProductVariantPrice.NewProductPrice(request.ProductVariantPrice),
            ColorCode.Create(request.ColorCode),
            ProductVariantDescription.Create(request.ProductVariantDescription),
            ProductId.FromGuid(request.ProductId),
            string.Empty,
            IsBaseVariant.False);
        
        await _productRepository.AddProductVariant(productVariant);

        await _unitOfWork.SaveChangeAsync();

        return Result.Success();
    }
}
