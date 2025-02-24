using Domain.Entities.Categories;
using Domain.IRepositories;
using Domain.IRepositories.CategoryRepositories;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Events.CategoryEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Infrastructure.Consumers.CategoryMessageConsumer;

internal sealed class CategoryCreatedMessageConsumer : IConsumer<CategoryCreatedEvent>
{
    #region Dependency
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    private readonly ILogger<CategoryCreatedMessageConsumer> _logger;
    public CategoryCreatedMessageConsumer(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork,
        ILogger<CategoryCreatedMessageConsumer> logger,
        IOutBoxMessageServices outBoxMessageServices)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _outBoxMessageServices = outBoxMessageServices;
    }
    #endregion
    public async Task Consume(ConsumeContext<CategoryCreatedEvent> context)
    {
        //Log start
        _logger.LogInformation(
            "Consuming CategoryCreatedEvent for categoryId: {CategoryId}",
            context.Message.categoryId);
        try
        {
            var category = ConvertEventToCategory(context.Message);
            await _categoryRepository.AddCategoryToPosgreSQL(category);
            var result = await _unitOfWork.SaveChangesAsync();

        }
        catch (Exception ex)
        {
            //Cập nhật trạng thái outbox message
            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                context.Message.messageId,
                OutboxMessageStatus.Failed,
                "Failed to consume CategoryCreatedEvent",
                DateTime.UtcNow);

            await _unitOfWork.Commit();

            //Log Error
            _logger.LogError(
                ex,
                "Failed to consume CategoryCreatedEvent for categoryId: {CategoryId}",
                context.Message.categoryId);

            throw; // Re-throw exception to mark the message as failed
        }

        await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
            context.Message.messageId,
            OutboxMessageStatus.Processed,
            string.Empty,
            DateTime.UtcNow);

        await _unitOfWork.Commit();

        //Log End
        _logger.LogInformation(
            "Successfully consumed CategoryCreatedEvent for categoryId: {CategoryId}",
            context.Message.categoryId);
    }

    private Category ConvertEventToCategory(CategoryCreatedEvent categoryCreatedEvent)
    {
        return Category.FromExisting(
            CategoryId.FromGuid(categoryCreatedEvent.categoryId),
            CategoryName.NewCategoryName(categoryCreatedEvent.categoryName),
            categoryCreatedEvent.imageUrl,
            categoryCreatedEvent.categoryStatus
        );
    }
}
