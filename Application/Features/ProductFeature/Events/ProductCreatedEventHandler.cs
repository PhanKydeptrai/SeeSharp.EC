using Domain.Events.ProductEvents;
using MediatR;
using StackExchange.Redis;


namespace Application.Features.ProductFeature.Events;

internal sealed class ProductCreatedEventHandler : INotificationHandler<ProductCreatedEvent>
{
    private readonly IDatabase _redisDb;

    public ProductCreatedEventHandler(IConnectionMultiplexer redis)
    {
        _redisDb = redis.GetDatabase();
    }

    public async Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        string globalVersionKey = "version:ProductList:global";
        await _redisDb.StringIncrementAsync(globalVersionKey);
    }
}
