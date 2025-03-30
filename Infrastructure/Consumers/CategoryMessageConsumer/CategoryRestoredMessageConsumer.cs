using Domain.Entities.Categories;
using Domain.IRepositories;
using Domain.IRepositories.Products;
using Domain.IRepositories.CategoryRepositories;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Events.CategoryEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Infrastructure.Consumers.CategoryMessageConsumer;

internal sealed class CategoryRestoredMessageConsumer : IConsumer<CategoryRestoredEvent>
{
    #region Dependencies
    private readonly ILogger<CategoryRestoredMessageConsumer> _logger;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;
    public CategoryRestoredMessageConsumer(
        ILogger<CategoryRestoredMessageConsumer> logger,
        IOutBoxMessageServices outBoxMessageServices,
        IUnitOfWork unitOfWork,
        ICategoryRepository categoryRepository,
        IProductRepository productRepository)
    {
        _logger = logger;
        _outBoxMessageServices = outBoxMessageServices;
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
    }
    #endregion

    public async Task Consume(ConsumeContext<CategoryRestoredEvent> context)
    {
        //Log start and begin transaction----------------------------------
        _logger.LogInformation(
            "Consuming CategoryRestoredEvent for productid: {CategoryId}",
            context.Message.CategoryId);
        using var transaction = await _unitOfWork.BeginPostgreSQLTransaction();
        //----------------------------------------------------------------

        try
        {
            //Consume message--------------------
            var category = await _categoryRepository.GetCategoryByIdFromPostgreSQL(
                CategoryId.FromGuid(context.Message.CategoryId));
            category!.Restore();
            await _unitOfWork.SaveChangeAsync();
            await _productRepository.RestoreProductByCategoryFromPostgreSQL(
                CategoryId.FromGuid(context.Message.CategoryId));

            transaction.Commit();
            //-----------------------------------
        }
        catch (Exception ex) //Rollback transaction and update outbox message status
        {
            transaction.Rollback();
            //Update outbox message status------------------------------
            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                context.Message.MessageId,
                OutboxMessageStatus.Failed,
                "Failed to consume CategoryRestoredEvent",
                DateTime.UtcNow);

            await _unitOfWork.SaveChangeAsync();
            //----------------------------------------------------------

            //Log error-------------------------------------------------
            _logger.LogError(
                ex,
                "Failed to consume CategoryRestoredEvent for productid: {CategoryId}",
                context.Message.CategoryId);
            //----------------------------------------------------------
            throw; //Stop the message
        }

        //Cập nhật trạng thái hoàn thành outbox message -> Commit --
        await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
            context.Message.MessageId,
            OutboxMessageStatus.Processed,
            "Successfully consumed CategoryRestoredEvent",
            DateTime.UtcNow);

        await _unitOfWork.SaveChangeAsync();
        //----------------------------------------------------------

        //Log end------------------------------------------
        _logger.LogInformation(
            "Consumed CategoryRestoredEvent for productid: {CategoryId}",
            context.Message.CategoryId);
        //-------------------------------------------------
    }
}
