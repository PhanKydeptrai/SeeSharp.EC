using Domain.Events.ProductEvents;
using MediatR;
using StackExchange.Redis;

namespace Application.Features.ProductFeature.Events;

internal sealed class ProductUpdatedEventHandler : INotificationHandler<ProductUpdatedEvent>
{
    private readonly IDatabase _redisDb;

    public ProductUpdatedEventHandler(IDatabase redisDb)
    {
        _redisDb = redisDb;
    }

    public async Task Handle(ProductUpdatedEvent notification, CancellationToken cancellationToken)
    {
        string productVersionKey = $"ProductResponse:{notification.ProductId.Value}";
        string productVariantVersionKey = $"VariantResponse:{notification.VariantId.Value}";
        string categoryListVersionKey = $"version:ProductList:cat:{notification.CategoryId.Value}";

        var product = _redisDb.KeyDeleteAsync(productVersionKey);
        var productVariant = _redisDb.KeyDeleteAsync(productVariantVersionKey);
        var categoryListVersion = _redisDb.StringIncrementAsync(categoryListVersionKey);

        await Task.WhenAll(product, productVariant, categoryListVersion);
    }
}
