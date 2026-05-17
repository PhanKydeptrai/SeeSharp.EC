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

        var product = _redisDb.KeyDeleteAsync(productVersionKey);
        var productVariant = _redisDb.KeyDeleteAsync(productVariantVersionKey);

        await Task.WhenAll(product, productVariant);
    }
}
