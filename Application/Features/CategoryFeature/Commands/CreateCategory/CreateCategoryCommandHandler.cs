using Application.Abstractions.Messaging;
using Application.Services;
using Domain.Entities.Categories;
using Domain.IRepositories;
using Domain.IRepositories.CategoryRepositories;
using SharedKernel;

namespace Application.Features.CategoryFeature.Commands.CreateCategory;

internal class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, CategoryId>
{
    #region Dependency
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CloudinaryService _cloudinaryService;

    public CreateCategoryCommandHandler(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork,
        CloudinaryService cloudinaryService)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _cloudinaryService = cloudinaryService;
    }
    #endregion

    public async Task<Result<CategoryId>> Handle(
        CreateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        // Xử lý ảnh
        //--------------------
        string imageUrl = string.Empty;
        if (request.image != null)
        {

            //tạo memory stream từ file ảnh
            var memoryStream = new MemoryStream();
            await request.image.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            //Upload ảnh lên cloudinary
            
            var resultUpload = await _cloudinaryService.UploadAsync(memoryStream, request.image.FileName);
            imageUrl = resultUpload.SecureUrl.ToString(); //Nhận url ảnh từ cloudinary
            //Log
            Console.WriteLine(resultUpload.JsonObj);
        }

        //--------------------

        var category = Category.NewCategory(
            CategoryName.NewCategoryName(request.categoryName),
            imageUrl);

        await _categoryRepository.AddCategoryToPosgreSQL(category);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success(category.CategoryId);
    }
}
