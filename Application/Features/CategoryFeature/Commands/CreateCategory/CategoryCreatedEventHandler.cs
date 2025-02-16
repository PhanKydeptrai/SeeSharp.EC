using Domain.Utilities.Events.CategoryEvents;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.CategoryFeature.Commands.CreateCategory;

internal sealed class CategoryCreatedEventHandler : INotificationHandler<CategoryCreatedEvent>
{
    private readonly ILogger<CategoryCreatedEventHandler> _logger;
    public CategoryCreatedEventHandler(
        ILogger<CategoryCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(CategoryCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Start add new CategoryCreatedEvent to Outbox Message");

        
        throw new NotImplementedException();
    }
}
