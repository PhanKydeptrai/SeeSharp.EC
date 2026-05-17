using Application.Abstractions.Messaging;
using Application.Services;
using Domain.Entities.Categories;
using Domain.Events.CategoryEvents;
using Domain.IRepositories;
using Domain.IRepositories.CategoryRepositories;
using MediatR;
using SharedKernel;

namespace Application.Features.CategoryFeature.Commands.CreateCategory;

internal class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, CategoryId>
{
    #region Dependency
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CloudinaryService _cloudinaryService;
    private readonly IPublisher _publisher;

    public CreateCategoryCommandHandler(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork,
        CloudinaryService cloudinaryService,
        IPublisher publisher)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _cloudinaryService = cloudinaryService;
        _publisher = publisher;
    }
    #endregion

    public async Task<Result<CategoryId>> Handle(
        CreateCategoryCommand request,
        CancellationToken cancellationToken)
    {

        string imageUrl = string.Empty;
        if (request.image is not null)
        {
            imageUrl = await _cloudinaryService.UploadNewImage(request.image);
        }

        var category = Category.NewCategory(
            CategoryName.NewCategoryName(request.categoryName),
            imageUrl);

        await _categoryRepository.AddCategoryToPosgreSQL(category);
        await _unitOfWork.SaveChangesAsync();
        await _publisher.Publish(new CategoryCreatedEvent(category.CategoryId), cancellationToken);
        return Result.Success(category.CategoryId);
    }
}
