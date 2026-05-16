using Application.Abstractions.Messaging;
using Application.Services;
using Domain.Entities.Products;
using Domain.Entities.ProductVariants;
using Domain.Events.ProductEvents;
using Domain.Events.ProductVariantEvents;
using Domain.IRepositories;
using Domain.IRepositories.Products;
using MediatR;
using SharedKernel;

namespace Application.Features.ProductFeature.Commands.CreateProductVariant;

internal sealed class CreateProductVariantCommandHandler : ICommandHandler<CreateProductVariantCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly CloudinaryService _cloudinaryService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublisher _publisher;
    public CreateProductVariantCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        CloudinaryService cloudinaryService,
        IPublisher publisher)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _cloudinaryService = cloudinaryService;
        _publisher = publisher;
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

        await _publisher.Publish(new ProductVariantCreatedEvent(productVariant.ProductVariantId, productVariant.ProductId), cancellationToken);

        return Result.Success();
    }
}
