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

namespace Application.Features.CategoryFeature.Commands.DeleteCategory;

internal sealed class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand>
{
    //FLOW: Get category by id from database -> Update category status -> Add Outbox message -> Commit -> Publish event
    private readonly ICategoryRepository _categoryRepository;
    private readonly IOutBoxMessageServices _oubBoxService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventBus _eventBus;
    public DeleteCategoryCommandHandler(
        ICategoryRepository categoryRepository,
        IOutBoxMessageServices oubBoxService,
        IUnitOfWork unitOfWork,
        IEventBus eventBus)
    {
        _categoryRepository = categoryRepository;
        _oubBoxService = oubBoxService;
        _unitOfWork = unitOfWork;
        _eventBus = eventBus;
    }

    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var categoryId = CategoryId.FromGuid(request.categoryId);
        var (category, failure) = await GetCategoryByIdAsync(CategoryId.FromGuid(categoryId));

        if (category is null)
        {
            return failure!;
        }

        category.Delete();

        var message = CreateCategoryDeletedEvent(category.CategoryId);

        await OutboxMessageExtentions.InsertOutboxMessageAsync(
            message.messageId,
            message,
            _oubBoxService);

        await _unitOfWork.Commit();
    
        await _eventBus.PublishAsync(message);
        
        return Result.Success();


    }
    //----------------------------------------------------------------
    private CategoryDeletedEvent CreateCategoryDeletedEvent(Guid categoryId)
    {
        return new CategoryDeletedEvent(categoryId, Ulid.NewUlid().ToGuid());
    }

    private async Task<(Category? category, Result? failure)> GetCategoryByIdAsync(CategoryId categoryId)
    {
        var category = await _categoryRepository.GetCategoryByIdFromMySQL(categoryId);

        if (category is null)
        {
            return (null, Result.Failure(CategoryErrors.NotFound(categoryId)));
        }

        if (category.CategoryStatus == CategoryStatus.Deleted)
        {
            return (null, Result.Failure(CategoryErrors.Deleted(categoryId)));
        }

        return (category, null);
    }
}