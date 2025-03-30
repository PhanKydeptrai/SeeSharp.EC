using Domain.Entities.Customers;
using Domain.Entities.Products;
using Domain.Entities.WishItems;
using Domain.IRepositories;
using Domain.IRepositories.WishItems;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Events.WishListEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Infrastructure.Consumers.WishListMessageConsumer;

internal sealed class AddWishItemMessageConsumer : IConsumer<AddWishItemEvent>
{
    private readonly ILogger<AddWishItemMessageConsumer> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWishItemRepository _wishItemRepository;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    public AddWishItemMessageConsumer(
        ILogger<AddWishItemMessageConsumer> logger,
        IOutBoxMessageServices outBoxMessageServices,
        IUnitOfWork unitOfWork,
        IWishItemRepository wishItemRepository)
    {
        _logger = logger;
        _outBoxMessageServices = outBoxMessageServices;
        _unitOfWork = unitOfWork;
        _wishItemRepository = wishItemRepository;
    }

    public async Task Consume(ConsumeContext<AddWishItemEvent> context)
    {
        //Log start-----------------------------------------------------
        _logger.LogInformation(
            "Consuming AddWishItemEvent for CustomerId: {CustomerId}",
            context.Message.CustomerId);
        //--------------------------------------------------------------

        try
        {
            //Consume message            
            var wishItem = WishItem.FromExisting(
                WishItemId.FromGuid(context.Message.WishItemId), 
                CustomerId.FromGuid(context.Message.CustomerId), 
                ProductId.FromGuid(context.Message.ProductId));
                
            await _wishItemRepository.AddWishItemToPostgreSQL(wishItem);
            await _unitOfWork.SaveChangeAsync();
            //---------------------------------------------------------------
        }
        catch (Exception ex)
        {
            //Update outbox message status------------------------------
            await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
                context.Message.MessageId,
                OutboxMessageStatus.Failed,
                "Failed to consume AddWishItemEvent",
                DateTime.UtcNow);

            await _unitOfWork.SaveChangeAsync();
            //----------------------------------------------------------

            //Log error-------------------------------------------------
            _logger.LogError(
                ex,
                "Failed to consume AddWishItemEvent for CustomerId: {CustomerId}",
                context.Message.CustomerId);
            //----------------------------------------------------------
            throw; //Stop the message
        }

        //Cập nhật trạng thái hoàn thành outbox message -> Commit --
        await _outBoxMessageServices.UpdateOutStatusBoxMessageAsync(
            context.Message.MessageId,
            OutboxMessageStatus.Processed,
            "Successfully consumed AddWishItemEvent",
            DateTime.UtcNow);

        await _unitOfWork.SaveChangeAsync();
        //----------------------------------------------------------

        //Log end------------------------------------------
        _logger.LogInformation(
            "Consumed AddWishItemEvent for CustomerId: {CustomerId}",
            context.Message.CustomerId);
        //-------------------------------------------------
    }
}
