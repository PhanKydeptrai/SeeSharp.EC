using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Application.Outbox;
using Domain.Entities.Categories;
using Domain.IRepositories;
using Domain.IRepositories.Categories;
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
    private readonly IProductRepository _productRepository;
    private readonly IOutBoxMessageServices _oubBoxService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventBus _eventBus;

    public DeleteCategoryCommandHandler(
        ICategoryRepository categoryRepository,
        IOutBoxMessageServices oubBoxService,
        IUnitOfWork unitOfWork,
        IEventBus eventBus,
        IProductRepository productRepository)
    {
        _categoryRepository = categoryRepository;
        _oubBoxService = oubBoxService;
        _unitOfWork = unitOfWork;
        _eventBus = eventBus;
        _productRepository = productRepository;
    }

    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        //Start transaction
        using var transaction = await _unitOfWork.BeginMySQLTransaction();
        var categoryId = CategoryId.FromGuid(request.categoryId);

        //Process delete category
        var (category, failure) = await GetCategoryByIdAsync(CategoryId.FromGuid(categoryId));
        if (category is null)
        {
            transaction.Rollback();
            return failure!;
        }
        category.Delete();

        //Insert outbox message
        var message = CreateCategoryDeletedEvent(category.CategoryId);
        await OutboxMessageExtentions.InsertOutboxMessageAsync(
            message.messageId,
            message,
            _oubBoxService);
        await _unitOfWork.SaveToMySQL();

        //Delete all products in this category
        await _productRepository.DeleteProductByCategoryFromMySQL(category.CategoryId);
        transaction.Commit();
        
        //Publish event
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