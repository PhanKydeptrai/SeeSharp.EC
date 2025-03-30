using Domain.Entities.Products;
using Domain.IRepositories;
using Domain.IRepositories.Products;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Events.ProductEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Infrastructure.Consumers.ProductMessageConsumer;

internal sealed class ProductRestoredMessageConsumer : IConsumer<ProductRestoredEvent>
{
    #region Dependency Injection
    private readonly ILogger<ProductRestoredEvent> _logger;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    public ProductRestoredMessageConsumer(
        ILogger<ProductRestoredEvent> logger,
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        IOutBoxMessageServices outBoxMessageServices)
    {
        _logger = logger;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _outBoxMessageServices = outBoxMessageServices;
    }
    #endregion
    //FLOW: Log start -> Get product from PostgreSQL -> Restore product -> Save changes -> Update outbox message status -> Commit -> Log end
    public async Task Consume(ConsumeContext<ProductRestoredEvent> context)
    {
        //Log start-----------------------------------------------------
        _logger.LogInformation(
            "Consuming ProductRestoredEvent for productid: {ProductId}",
            context.Message.ProductId);
        //--------------------------------------------------------------

        try
        {
            //Get product from PostgreSQL -> Restore product -> Save changes-
            var product = await _productRepository.GetProductFromPostgreSQL(
               ProductId.FromGuid(context.Message.ProductId));
            product!.Restore();
            await _unitOfWork.SaveChangeAsync();
            //---------------------------------------------------------------
        }
        catch (Exception ex)
        {
            //Update outbox message status------------------------------
            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                context.Message.MessageId,
                OutboxMessageStatus.Failed,
                "Failed to consume ProductRestoredEvent",
                DateTime.UtcNow);

            await _unitOfWork.SaveChangeAsync(); 
            //----------------------------------------------------------
            
            //Log error-------------------------------------------------------------
            _logger.LogError(
                ex,
                "Failed to consume ProductRestoredEvent for productid: {ProductId}",
                context.Message.ProductId);
            //-----------------------------------------------------------------------
            throw; //Stop the message
        }

        //Cập nhật trạng thái hoàn thành outbox message -> Commit --
        await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
            context.Message.MessageId,
            OutboxMessageStatus.Processed,
            "Successfully consumed ProductRestoredEvent",
            DateTime.UtcNow);

        await _unitOfWork.SaveChangeAsync();
        //----------------------------------------------------------

        //Log end------------------------------------------------------
        _logger.LogInformation(
            "Consumed ProductRestoredEvent for productid: {ProductId}",
            context.Message.ProductId);
        //--------------------------------------------------------------
    }
}
