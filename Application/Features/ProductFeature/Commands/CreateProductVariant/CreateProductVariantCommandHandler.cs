using Application.Abstractions.Messaging;
using Application.Services;
using Domain.Entities.Products;
using Domain.Entities.ProductVariants;
using Domain.IRepositories;
using Domain.IRepositories.Products;
using SharedKernel;

namespace Application.Features.ProductFeature.Commands.CreateProductVariant;

internal sealed class CreateProductVariantCommandHandler : ICommandHandler<CreateProductVariantCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly CloudinaryService _cloudinaryService;
    private readonly IUnitOfWork _unitOfWork;
    public CreateProductVariantCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        CloudinaryService cloudinaryService)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _cloudinaryService = cloudinaryService;
    }

    public async Task<Result> Handle(
        CreateProductVariantCommand request,
        CancellationToken cancellationToken)
    {
        string imageUrl = string.Empty;
        if (request.Image != null)
        {
            imageUrl = await _cloudinaryService.UploadNewImage(request.Image);
        }
        var productVariant = ProductVariant.Create(
            VariantName.NewVariantName(request.VariantName),
            ProductVariantPrice.NewProductPrice(request.ProductVariantPrice),
            ColorCode.Create(request.ColorCode),
            ProductVariantDescription.Create(request.ProductVariantDescription),
            ProductId.FromGuid(request.ProductId),
            imageUrl,
            IsBaseVariant.False);

        await _productRepository.AddProductVariant(productVariant);

        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
