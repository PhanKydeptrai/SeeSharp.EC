using Application.Outbox;
using Domain.IRepositories;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Events.CategoryEvents;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.CategoryFeature.Commands.UpdateCategory;

public class CategoryUpdatedEventHandler : INotificationHandler<CategoryUpdatedEvent>
{
    private readonly ILogger<CategoryUpdatedEventHandler> _logger;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    private readonly IUnitOfWork _unitOfWork;
    public CategoryUpdatedEventHandler(
        ILogger<CategoryUpdatedEventHandler> logger,
        IUnitOfWork unitOfWork,
        IOutBoxMessageServices outBoxMessageServices)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _outBoxMessageServices = outBoxMessageServices;
    }

    public async Task Handle(CategoryUpdatedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Start add new CategoryUpdatedEvent to Outbox Message");

            await OutboxMessageExtentions.InsertOutboxMessageAsync(
                notification.messageId, 
                notification, 
                _outBoxMessageServices, 
                _unitOfWork);

            _logger.LogInformation("End add new CategoryUpdatedEvent to Outbox Message");
        }
        catch (Exception ex)
        {
            //FIXME: Chưa tối ưu kiểu dữ liệu
            _logger.LogError(ex.ToString());
        }
    }
}
