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
        // Xử lý ảnh
        //--------------------
        string imageUrl = string.Empty;
        if (request.Image != null)
        {

            //tạo memory stream từ file ảnh
            var memoryStream = new MemoryStream();
            await request.Image.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            //Upload ảnh lên cloudinary
            
            var resultUpload = await _cloudinaryService.UploadAsync(memoryStream, request.Image.FileName);
            imageUrl = resultUpload.SecureUrl.ToString(); //Nhận url ảnh từ cloudinary
            //Log
            Console.WriteLine(resultUpload.JsonObj);
        }

        //--------------------
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
