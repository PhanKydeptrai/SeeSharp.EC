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
        string categoryListVersionKey = $"version:ProductList:cat:{notification.CategoryId.Value}";
        string variantListVersionKey = $"version:VariantList:global";

        var globalVersion = _redisDb.StringIncrementAsync(globalVersionKey);
        var categoryListVersion = _redisDb.StringIncrementAsync(categoryListVersionKey);
        var variantListVersion = _redisDb.StringIncrementAsync(variantListVersionKey);
        await Task.WhenAll(globalVersion, categoryListVersion, variantListVersion);
    }
}
