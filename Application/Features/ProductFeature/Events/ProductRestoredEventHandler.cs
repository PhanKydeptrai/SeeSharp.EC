using Domain.Events.ProductEvents;
using MediatR;
using StackExchange.Redis;

namespace Application.Features.ProductFeature.Events;

internal sealed class ProductRestoredEventHandler : INotificationHandler<ProductRestoredEvent>
{
    private readonly IConnectionMultiplexer _redis;

    public ProductRestoredEventHandler(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public async Task Handle(ProductRestoredEvent notification, CancellationToken cancellationToken)
    {
        var db = _redis.GetDatabase();

        string productKey = $"ProductResponse:{notification.ProductId.Value}";
        await db.KeyDeleteAsync(productKey);

        await Task.WhenAll(
            db.StringIncrementAsync("version:ProductList:global"),
            db.StringIncrementAsync("version:VariantList:global")
        );

        var server = _redis.GetServer(_redis.GetEndPoints().First());
        
        // Xóa những bản ghi variant response chứa ProductId này
        // Lấy tất cả các key của ProductVariantResponse
        var productVariantKeys = server.Keys(pattern: "ProductVariantResponse:*").ToArray();
        var keysToDelete = new List<RedisKey>();

        foreach (var key in productVariantKeys)
        {
            var value = await db.StringGetAsync(key);
            if (value.HasValue && value.ToString().Contains(notification.ProductId.Value.ToString()))
            {
                keysToDelete.Add(key);
            }
        }

        if (keysToDelete.Any())
        {
            await db.KeyDeleteAsync(keysToDelete.ToArray()); 
        }
    }
}
