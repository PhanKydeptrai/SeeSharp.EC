using Domain.Events.CategoryEvents;
using MediatR;
using StackExchange.Redis;

namespace Application.Features.CategoryFeature.Events;

internal sealed class CategoryUpdatedEventHandler : INotificationHandler<CategoryUpdatedEvent>
{
    private readonly IDatabase _redisDb;

    public CategoryUpdatedEventHandler(IDatabase redis)
    {
        _redisDb = redis;
    }

    public async Task Handle(CategoryUpdatedEvent notification, CancellationToken cancellationToken)
    {
        string globalVersionKey = "version:CategoryList:global";
        string categoryDetailsKey = $"CategoryResponse:{notification.CategoryId.Value}";

        var incrementVersion = _redisDb.StringIncrementAsync(globalVersionKey);
        var deleteCache = _redisDb.KeyDeleteAsync(categoryDetailsKey);

        await Task.WhenAll(incrementVersion, deleteCache);
    }
}
