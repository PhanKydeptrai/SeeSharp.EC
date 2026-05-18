using Domain.Events.ProductEvents;
using MediatR;
using StackExchange.Redis;


namespace Application.Features.ProductFeature.Events;

internal sealed class ProductCreatedEventHandler : INotificationHandler<ProductCreatedEvent>
{
    private readonly IDatabase _redisDb;

    public ProductCreatedEventHandler(IDatabase redis)
    {
        _redisDb = redis;
    }

    public async Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        string globalVersionKey = "version:ProductList:global";
        string categoryListVersionKey = $"version:ProductList:cat:{notification.CategoryId.Value}";
        string variantListVersionKey = $"version:VariantList:global";
        string variantCategoryListVersionKey = $"version:VariantList:cat:{notification.CategoryId.Value}";

        var globalVersion = _redisDb.StringIncrementAsync(globalVersionKey);
        var categoryListVersion = _redisDb.StringIncrementAsync(categoryListVersionKey);
        var variantListVersion = _redisDb.StringIncrementAsync(variantListVersionKey);
        var variantCategoryListVersion = _redisDb.StringIncrementAsync(variantCategoryListVersionKey);

        await Task.WhenAll(globalVersion, categoryListVersion, variantListVersion, variantCategoryListVersion);
    }
}
