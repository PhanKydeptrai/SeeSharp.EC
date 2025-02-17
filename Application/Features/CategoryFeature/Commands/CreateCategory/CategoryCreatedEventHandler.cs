using Application.Outbox;
using Domain.IRepositories;
using Domain.OutboxMessages.Services;
using Domain.Utilities.Events.CategoryEvents;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.CategoryFeature.Commands.CreateCategory;

internal sealed class CategoryCreatedEventHandler : INotificationHandler<CategoryCreatedEvent>
{
    private readonly ILogger<CategoryCreatedEventHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOutBoxMessageServices _outBoxMessageServices;
    public CategoryCreatedEventHandler(
        ILogger<CategoryCreatedEventHandler> logger,
        IUnitOfWork unitOfWork,
        IOutBoxMessageServices outBoxMessageServices)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _outBoxMessageServices = outBoxMessageServices;
    }

    public async Task Handle(CategoryCreatedEvent notification, CancellationToken cancellationToken)
    { 
        try
        {
            _logger.LogInformation("Start add new CategoryCreatedEvent to Outbox Message");

            await OutboxMessageExtentions.InsertOutboxMessageAsync(notification, _outBoxMessageServices, _unitOfWork);
                
            _logger.LogInformation("End add new CategoryCreatedEvent to Outbox Message");
        }
        catch (Exception ex)
        {
            //FIXME: Chưa tối ưu kiểu dữ liệu
            _logger.LogError(ex.ToString()); 
        }
        
    }
}
