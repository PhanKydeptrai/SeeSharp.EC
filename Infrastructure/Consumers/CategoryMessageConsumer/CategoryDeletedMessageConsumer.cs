using Domain.Entities.Categories;
using Domain.IRepositories;
using Domain.IRepositories.Products;
using Domain.IRepositories.CategoryRepositories;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Events.CategoryEvents;
using MassTransit;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Infrastructure.Consumers.CategoryMessageConsumer;

internal sealed class CategoryDeletedMessageConsumer : IConsumer<CategoryDeletedEvent>
{
    #region Dependency
    private readonly ILogger<CategoryDeletedMessageConsumer> _logger;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    private readonly IDistributedCache _distributedCache;
    public CategoryDeletedMessageConsumer(
        ILogger<CategoryDeletedMessageConsumer> logger,
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork,
        IOutBoxMessageServices outBoxMessageServices,
        IDistributedCache distributedCache,
        IProductRepository productRepository)
    {
        _logger = logger;
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _outBoxMessageServices = outBoxMessageServices;
        _distributedCache = distributedCache;
        _productRepository = productRepository;
    }
    #endregion

    public async Task Consume(ConsumeContext<CategoryDeletedEvent> context)
    {
        using var transaction = await _unitOfWork.BeginPostgreSQLTransaction();
        //Log start
        _logger.LogInformation(
            "Consuming CategoryDeletedEvent for categoryId: {CategoryId}",
            context.Message.categoryId);
        try
        {
            var category = await _categoryRepository.GetCategoryByIdFromPostgreSQL(
            CategoryId.FromGuid(context.Message.categoryId));
            category!.Delete();
            await _unitOfWork.SaveChangeAsync();
            await _productRepository.DeleteProductByCategoryFromPosgreSQL(category.CategoryId);
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();

            //Failed to consume CategoryDeletedEvent => Update outbox message status
            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                    context.Message.messageId,
                    OutboxMessageStatus.Failed,
                    "Failed to consume CategoryDeletedEvent",
                    DateTime.UtcNow);

            await _unitOfWork.SaveChangeAsync();

            _logger.LogError(
                ex,
                "Failed to consume CategoryDeletedEvent for categoryId: {CategoryId}",
                context.Message.categoryId);

            throw; //Stop the message
        }
        
        //Cập nhật trạng thái outbox message
        await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
            context.Message.messageId,
            OutboxMessageStatus.Processed,
            "Successfully consumed CategoryDeletedEvent",
            DateTime.UtcNow);

        await _unitOfWork.SaveChangeAsync();

        //Ivalidating cache
        string cacheKey = $"CategoryResponse:{context.Message.categoryId}"; 
        await _distributedCache.RemoveAsync(cacheKey);

        //Log End
        _logger.LogInformation(
            "Successfully consumed CategoryDeletedEvent for categoryId: {CategoryId}",
            context.Message.categoryId);

    }

}
