using Domain.Entities.Categories;
using Domain.Entities.Products;
using Domain.IRepositories;
using Domain.IRepositories.Categories;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Events.ProductEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Infrastructure.Consumers.ProductMessageConsumer;
internal sealed class ProductUpdatedMessageConsumer : IConsumer<ProductUpdatedEvent>
{
    #region Dependency
    private readonly ILogger<ProductUpdatedMessageConsumer> _logger;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOutBoxMessageServices _outBoxMessageServices;

    public ProductUpdatedMessageConsumer(
        ILogger<ProductUpdatedMessageConsumer> logger,
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
    public async Task Consume(ConsumeContext<ProductUpdatedEvent> context)
    {
        //Log start
        _logger.LogInformation(
            "Consuming ProductUpdatedEvent for productid: {ProductId}",
            context.Message.ProductId);

        try
        {

            var product = await _productRepository.GetProductFromPosgreSQL(
                ProductId.FromGuid(context.Message.ProductId));

            UpdateProduct(product!, context.Message);

            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                    context.Message.MessageId,
                    OutboxMessageStatus.Failed,
                    "Failed to consume ProductUpdatedEvent",
                    DateTime.UtcNow);

            await _unitOfWork.Commit();

            //Log error
            _logger.LogError(
                ex,
                "Failed to consume ProductUpdatedEvent for productid: {ProductId}",
                context.Message.ProductId);
                
                throw; //Stop the message
        }

        //Cập nhật trạng thái hoàn thành outbox message
        await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
            context.Message.MessageId,
            OutboxMessageStatus.Processed,
            "Successfully consumed ProductUpdatedEvent",
            DateTime.UtcNow);

        await _unitOfWork.Commit();

        //Log end
        _logger.LogInformation(
            "Successfully consumed ProductUpdatedEvent for productid: {ProductId}",
            context.Message.ProductId);

    }

    //--------------------------------------------------------------------------------
    private void UpdateProduct(Product product, ProductUpdatedEvent request)
    {
        product.Update(
            ProductName.NewProductName(request.ProductName),
            string.Empty, //TODO: Xử lý ảnh
            request.Description,
            ProductPrice.NewProductPrice(request.Price),
            product.ProductStatus,
            CategoryId.FromGuid(request.CategoryId));
    }

}
