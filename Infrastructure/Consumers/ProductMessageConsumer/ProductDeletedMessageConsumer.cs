using Domain.Entities.Products;
using Domain.IRepositories;
using Domain.IRepositories.Categories;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Events.ProductEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Infrastructure.Consumers.ProductMessageConsumer;

internal sealed class ProductDeletedMessageConsumer : IConsumer<ProductDeletedEvent>
{
    private readonly ILogger<ProductDeletedMessageConsumer> _logger;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOutBoxMessageServices _outBoxMessageServices;

    public ProductDeletedMessageConsumer(
        IOutBoxMessageServices outBoxMessageServices,
        IUnitOfWork unitOfWork,
        IProductRepository productRepository,
        ILogger<ProductDeletedMessageConsumer> logger)
    {
        _outBoxMessageServices = outBoxMessageServices;
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ProductDeletedEvent> context)
    {
        //Log start
        _logger.LogInformation(
            "Consuming ProductDeletedEvent for productid: {ProductId}",
            context.Message.ProductId);

        try
        {
            var product = await _productRepository.GetProductFromPostgreSQL(
               ProductId.FromGuid(context.Message.ProductId));
            product!.Delete();
            await _unitOfWork.SaveToPostgreSQL();
        }
        catch (Exception ex)
        {
            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                context.Message.MessageId,
                OutboxMessageStatus.Failed,
                "Failed to consume ProductDeletedEvent",
                DateTime.UtcNow);

            await _unitOfWork.SaveToMySQL();

            //Log error
            _logger.LogError(
                ex,
                "Failed to consume ProductDeletedEvent for productid: {ProductId}",
                context.Message.ProductId);

            throw; //Stop the message
        }
        
        //Cập nhật trạng thái hoàn thành outbox message
        await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
            context.Message.MessageId,
            OutboxMessageStatus.Processed,
            "Successfully consumed ProductDeletedEvent",
            DateTime.UtcNow);

        await _unitOfWork.SaveToMySQL();
        //Log end
        _logger.LogInformation(
            "Consumed ProductDeletedEvent for productid: {ProductId}",
            context.Message.ProductId);
    }
}
