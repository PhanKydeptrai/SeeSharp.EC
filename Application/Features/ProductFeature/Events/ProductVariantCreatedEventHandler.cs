using Domain.Events.ProductVariantEvents;
using MediatR;
using StackExchange.Redis;

namespace Application.Features.ProductFeature.Events;

internal sealed class ProductVariantCreatedEventHandler : INotificationHandler<ProductVariantCreatedEvent>
{
    private readonly IDatabase _redisDb;
    public ProductVariantCreatedEventHandler(IDatabase redisDb)
    {
        _redisDb = redisDb;
    }

    public async Task Handle(ProductVariantCreatedEvent notification, CancellationToken cancellationToken)
    {

        string productListVersionKey = "version:ProductList:global";
        string productResponseKey = $"ProductResponse:{notification.ProductId.Value}";
        string variantListVersionKey = $"version:VariantList:global";
        string productListVersionKeyForCategory = $"version:ProductList:cat:{notification.CategoryId.Value}";
        string variantListVersionKeyForCategory = $"version:VariantList:cat:{notification.CategoryId.Value}";
        

        var incrementProductList = _redisDb.StringIncrementAsync(productListVersionKey);
        var deleteProductResponse = _redisDb.KeyDeleteAsync(productResponseKey);
        var incrementVariantList = _redisDb.StringIncrementAsync(variantListVersionKey);
        var incrementCategoryProductListVersion = _redisDb.StringIncrementAsync(productListVersionKeyForCategory);
        var incrementCategoryVariantListVersion = _redisDb.StringIncrementAsync(variantListVersionKeyForCategory);

        await Task.WhenAll(
            incrementProductList, 
            deleteProductResponse, 
            incrementVariantList, 
            incrementCategoryProductListVersion,
            incrementCategoryVariantListVersion);
    }
}
