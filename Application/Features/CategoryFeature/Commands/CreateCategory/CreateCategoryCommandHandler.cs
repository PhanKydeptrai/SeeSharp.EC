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

namespace Application.Features.CategoryFeature.Commands.CreateCategory;

internal class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, CategoryId>
{
    #region Dependency
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventBus _eventBus;
    private readonly IOutBoxMessageServices _outboxservice;

    public CreateCategoryCommandHandler(
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
    #endregion
    //FLOW: Create category -> Add category to MySQL -> Add Outbox message -> Commit -> Publish event
    public async Task<Result<CategoryId>> Handle(
        CreateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        //TODO: Xử lý ảnh
        //--------------------

        string imageUrl = string.Empty;
        //--------------------

        var category = Category.NewCategory(
            CategoryName.NewCategoryName(request.categoryName),
            imageUrl);

        var message = CreateCategoryCreatedEvent(category);
        //Add category to MySQL
        await _categoryRepository.AddCategoryToMySQL(category);
        //Add Outbox message
        await OutboxMessageExtentions.InsertOutboxMessageAsync(
            message.messageId,
            message,
            _outboxservice);

        if (await _unitOfWork.SaveToMySQL() > 0)
        {
            await _eventBus.PublishAsync(message);
            return Result.Success(category.CategoryId);
        }
        
        return Result.Failure<CategoryId>(CategoryErrors.Failure(category.CategoryId));
    }
    
    private CategoryCreatedEvent CreateCategoryCreatedEvent(Category category)
    {
        return new CategoryCreatedEvent(
            category.CategoryId.Value,
            category.CategoryName.Value,
            category.ImageUrl ?? string.Empty,
            category.CategoryStatus,
            category.IsDefault,
            Ulid.NewUlid().ToGuid());
    }
}
