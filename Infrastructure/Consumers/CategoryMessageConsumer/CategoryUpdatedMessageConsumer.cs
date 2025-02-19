using Domain.Entities.Categories;
using Domain.IRepositories.CategoryRepositories;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Events.CategoryEvents;
using MassTransit;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Infrastructure.Consumers.CategoryMessageConsumer;

internal sealed class CategoryUpdatedMessageConsumer : IConsumer<CategoryUpdatedEvent>
{
    private readonly ILogger<CategoryCreatedMessageConsumer> _logger;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IDistributedCache _distributedCache;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    public CategoryUpdatedMessageConsumer(
        ILogger<CategoryCreatedMessageConsumer> logger,
        ICategoryRepository categoryRepository,
        IOutBoxMessageServices outBoxMessageServices,
        IDistributedCache distributedCache)
    {
        _logger = logger;
        _categoryRepository = categoryRepository;
        _outBoxMessageServices = outBoxMessageServices;
        _distributedCache = distributedCache;
    }

    public async Task Consume(ConsumeContext<CategoryUpdatedEvent> context)
    {
        _logger.LogInformation(
            "Consuming CategoryUpdatedEvent for categoryId: {CategoryId}",
            context.Message.categoryId);

        var category = ConvertEventToCategory(context.Message);
        var result = await _categoryRepository.UpdateCategoryToPosgreSQL(category);
        if (result <= 0)
        {
            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                    context.Message.messageId,
                    OutboxMessageStatus.Failed,
                    "Failed to consume CategoryUpdatedEvent",
                    DateTime.UtcNow);

            _logger.LogError(
                "Failed to consume CategoryUpdatedEvent for categoryId: {CategoryId}",
                context.Message.categoryId);

            throw new Exception("Category updated event consumed failed");
        }

        //Cập nhật trạng thái outbox message
        await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
            context.Message.messageId,
            OutboxMessageStatus.Failed,
            "Failed to consume CategoryUpdatedEvent",
            DateTime.UtcNow);

        //Ivalidating cache
        string cacheKey = $"CategoryResponse:{context.Message.categoryId}";
        await _distributedCache.RemoveAsync(cacheKey);

        _logger.LogInformation(
            "Successfully consumed CategoryUpdatedEvent for categoryId: {CategoryId}",
            context.Message.categoryId);
    }

    private Category ConvertEventToCategory(CategoryUpdatedEvent categoryCreatedEvent)
    {
        return Category.FromExisting(
            CategoryId.FromUlid(categoryCreatedEvent.categoryId),
            CategoryName.NewCategoryName(categoryCreatedEvent.categoryName),
            categoryCreatedEvent.imageUrl
        );
    }
}
