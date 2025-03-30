using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Application.Outbox;
using Domain.Entities.Categories;
using Domain.IRepositories;
using Domain.IRepositories.CategoryRepositories;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Errors;
using Domain.Utilities.Events.CategoryEvents;
using SharedKernel;

namespace Application.Features.CategoryFeature.Commands.UpdateCategory;
public class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IEventBus _eventBus;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOutBoxMessageServices _outboxservice;
    public UpdateCategoryCommandHandler(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork,
        IEventBus eventBus,
        IOutBoxMessageServices outboxservice)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _eventBus = eventBus;
        _outboxservice = outboxservice;
    }
    //FLOW: Get category by id from database -> Update category -> Add Outbox message -> Commit -> Publish event
    public async Task<Result> Handle(
        UpdateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var categoryId = CategoryId.FromGuid(request.categoryId);

        var (category, failure) = await GetCategoryByIdAsync(categoryId);

        if (category is null) return failure!;

        //Update category
        UpdateCategory(category, request);
        var message = CreateCategoryUpdatedEvent(category);

        await _unitOfWork.SaveChangeAsync(cancellationToken);

        return Result.Success();
    }

    //* Private methods
    private CategoryUpdatedEvent CreateCategoryUpdatedEvent(Category category)
    {
        return new CategoryUpdatedEvent(
                category.CategoryId.Value,
                category.CategoryName.Value,
                category.ImageUrl ?? string.Empty,
                category.CategoryStatus,
                Ulid.NewUlid().ToGuid());
    }
    private async Task<(Category? category, Result? failure)> GetCategoryByIdAsync(CategoryId categoryId)
    {
        var category = await _categoryRepository.GetCategoryByIdFromMySQL(categoryId);
        if (category is null) return (null, Result.Failure(CategoryErrors.NotFound(categoryId)));
        if (category.IsDefault)
        {
            return (null, Result.Failure(CategoryErrors.DefaultCategoryCannotBeUpdated(categoryId)));
        }
        return (category, null);
    }
    private void UpdateCategory(Category category, UpdateCategoryCommand request)
    {
        //TODO: Xử lý ảnh
        //--------------------


        //--------------------
        category.Update(
                CategoryName.FromString(request.categoryName),
                category.CategoryStatus,
                string.Empty); // TODO: Get image from request
    }
}
