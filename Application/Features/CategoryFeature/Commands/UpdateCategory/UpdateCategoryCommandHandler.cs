using Application.Abstractions.Messaging;
using Application.Services;
using Domain.Entities.Categories;
using Domain.IRepositories;
using Domain.IRepositories.CategoryRepositories;
using Domain.Utilities.Errors;
using SharedKernel;

namespace Application.Features.CategoryFeature.Commands.UpdateCategory;
internal class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CloudinaryService _cloudinaryService;
    public UpdateCategoryCommandHandler(
        ICategoryRepository categoryRepository,
        CloudinaryService cloudinaryService,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _cloudinaryService = cloudinaryService;
    }

    public async Task<Result> Handle(
        UpdateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var categoryId = CategoryId.FromGuid(request.categoryId);

        var (category, failure) = await GetCategoryByIdAsync(categoryId);

        if (category is null) return failure!;

        //Update category
        await UpdateCategoryAsync(category, request);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    //* Private methods
    private async Task<(Category? category, Result? failure)> GetCategoryByIdAsync(CategoryId categoryId)
    {
        var category = await _categoryRepository.GetCategoryByIdFromPostgreSQL(categoryId);
        if (category is null) return (null, Result.Failure(CategoryErrors.NotFound(categoryId)));
        if (category.IsDefault)
        {
            return (null, Result.Failure(CategoryErrors.DefaultCategoryCannotBeUpdated(categoryId)));
        }
        return (category, null);
    }
    private async Task UpdateCategoryAsync(Category category, UpdateCategoryCommand request)
    {   
        string newImageUrl = category.ImageUrl ?? string.Empty;
        if (request.categoryImage is not null)
        {
            //Xử lý lưu ảnh mới
            if (request.categoryImage != null)
            {
                newImageUrl = await _cloudinaryService.UploadNewImage(request.categoryImage);
            }
        }

        category.Update(
            CategoryName.FromString(request.categoryName),
            category.CategoryStatus,
            newImageUrl);
    }
}
