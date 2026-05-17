using Domain.Events.CategoryEvents;
using MediatR;
using StackExchange.Redis;

namespace Application.Features.CategoryFeature.Events;

internal sealed class CategoryCreatedEventHandler : INotificationHandler<CategoryCreatedEvent>
{
    private readonly IDatabase _redisDb;

    public CategoryCreatedEventHandler(IDatabase redis)
    {
        _redisDb = redis;
    }

    public async Task Handle(CategoryCreatedEvent notification, CancellationToken cancellationToken)
    {
        string globalVersionKey = "version:CategoryList:global";
        await _redisDb.StringIncrementAsync(globalVersionKey);
    }
}
