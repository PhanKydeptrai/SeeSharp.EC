using Domain.Events.ProductVariantEvents;
using MediatR;
using StackExchange.Redis;

namespace Application.Features.ProductFeature.Events;

internal sealed class ProductVariantRestoredEventHandler : INotificationHandler<ProductVariantRestoredEvent>
{
    private readonly IDatabase _redisDb;

    public ProductVariantRestoredEventHandler(IDatabase redisDb)
    {
        _redisDb = redisDb;
    }

    public async Task Handle(ProductVariantRestoredEvent notification, CancellationToken cancellationToken)
    {
        string variantResponseKey = $"ProductVariantResponse:{notification.ProductVariantId.Value}";
        string productResponseKey = $"ProductResponse:{notification.ProductId.Value}";
        string variantListVersionKey = "version:VariantList:global";
        string categoryVariantListVersionKey = $"version:VariantList:cat:{notification.CategoryId.Value}";
        
        var deleteVariantResponse = _redisDb.KeyDeleteAsync(variantResponseKey);
        var deleteProductResponse = _redisDb.KeyDeleteAsync(productResponseKey);
        var incrementVariantListVersion = _redisDb.StringIncrementAsync(variantListVersionKey);
        var incrementCategoryVariantListVersion = _redisDb.StringIncrementAsync(categoryVariantListVersionKey);

        await Task.WhenAll(
            deleteVariantResponse, 
            deleteProductResponse, 
            incrementVariantListVersion,
            incrementCategoryVariantListVersion);
    }
}
