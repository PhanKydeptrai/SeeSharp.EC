using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Domain.Entities.Categories;
using Domain.IRepositories;
using Domain.IRepositories.CategoryRepositories;
using Domain.Utilities.Errors;
using Domain.Utilities.Events.CategoryEvents;
using MediatR;
using SharedKernel;

namespace Application.Features.CategoryFeature.Commands.UpdateCategory;

public class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IPublisher _publisher;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateCategoryCommandHandler(
        ICategoryRepository categoryRepository,
        IPublisher publisher,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _publisher = publisher;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var categoryId = CategoryId.FromString(request.categoryId);

        var (category, failure) = await GetCategoryByIdAsync(categoryId);

        if (category is null)
        {
            return failure!;
        }

        UpdateCategory(category, request);

        int result = await _unitOfWork.Commit(cancellationToken);

        if (result <= 0)
        {
            return Result.Failure(CategoryErrors.Failure(category.CategoryId));
        }

        var message = new CategoryUpdatedEvent(
                category.CategoryId.Value,
                category.CategoryName.Value,
                category.ImageUrl ?? string.Empty,
                Ulid.NewUlid());

        await _publisher.Publish(message);
        return Result.Success();
    }

    //* Private methods
    private CategoryUpdatedEvent CreateCategoryUpdatedEvent()
    {
        throw new NotImplementedException();
    }
    private async Task<(Category? category, Result? failure)> GetCategoryByIdAsync(CategoryId categoryId)
    {
        var category = await _categoryRepository.GetCategoryByIdFromMySQL(categoryId);
        if (category is null)
        {
            return (null, Result.Failure(CategoryErrors.NotFound(categoryId)));
        }
        return (category, null);
    }

    private void UpdateCategory(Category category, UpdateCategoryCommand request)
    {
        Category.Update(category,
                CategoryName.FromString(request.categoryName),
                string.Empty); // TODO: Get image from request
    }
}
