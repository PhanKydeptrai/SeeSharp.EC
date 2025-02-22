using Domain.Entities.Categories;
using Domain.Entities.Products;
using Domain.IRepositories;
using Domain.IRepositories.Categories;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Events.ProductEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedKernel;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Consumers.ProductMessageConsumer;

internal sealed class ProductCreatedMessageConsumer : IConsumer<ProductCreatedEvent>
{
    private readonly ILogger<ProductCreatedMessageConsumer> _logger;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOutBoxMessageServices _outBoxMessageServices;

    public ProductCreatedMessageConsumer(
        ILogger<ProductCreatedMessageConsumer> logger,
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        IOutBoxMessageServices outBoxMessageServices)
    {
        _logger = logger;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _outBoxMessageServices = outBoxMessageServices;
    }

    public async Task Consume(ConsumeContext<ProductCreatedEvent> context)
    {
        //Log start
        _logger.LogInformation(
            "Consuming ProductCreatedEvent for productid: {ProductId}",
            context.Message.ProductId);

        var product = ConvertOutboxMessageToProduct(context.Message);
        await _productRepository.AddProductToPosgreSQL(product);

        var result = await _unitOfWork.SaveChangesAsync();

        if(result <= 0) //Nếu không thành công
        {
            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                    context.Message.MessageId,
                    OutboxMessageStatus.Failed,
                    "Failed to consume ProductCreatedEvent",
                    DateTime.UtcNow);

            await _unitOfWork.Commit();

            _logger.LogError(
                "Failed to consume ProductCreatedEvent for ProductId: {ProductId}",
                context.Message.ProductId);

            throw new Exception("Product updated event consumed failed");
        }

        //Cập nhật trạng thái outbox message
        await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
            context.Message.MessageId,
            OutboxMessageStatus.Processed,
            "Successfully consumed ProductCreatedEvent",
            DateTime.UtcNow);

        await _unitOfWork.Commit();

        //Log End
        _logger.LogInformation(
            "Successfully consumed CategoryDeletedEvent for categoryId: {CategoryId}",
            context.Message.ProductId);
    }


    private Product ConvertOutboxMessageToProduct(ProductCreatedEvent productCreatedEvent)
    {
        return Product.FromExisting(
            ProductId.FromUlid(productCreatedEvent.ProductId),
            ProductName.FromString(productCreatedEvent.ProductName),
            productCreatedEvent.ImageUrl,
            productCreatedEvent.Description,
            ProductPrice.FromDecimal(productCreatedEvent.Price),
            productCreatedEvent.ProductStatus,
            CategoryId.FromUlid(productCreatedEvent.CategoryId));
    }
}
