using Domain.Events.CategoryEvents;
using MediatR;
using StackExchange.Redis;

namespace Application.Features.CategoryFeature.Events;

internal sealed class CategoryDeletedEventHandler : INotificationHandler<CategoryDeletedEvent>
{
    private readonly IConnectionMultiplexer _redis;

    public CategoryDeletedEventHandler(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public async Task Handle(CategoryDeletedEvent notification, CancellationToken cancellationToken)
    {
        var db = _redis.GetDatabase();

        string categoryDetailsKey = $"CategoryResponse:{notification.CategoryId.Value}";
        await db.KeyDeleteAsync(categoryDetailsKey);

        await Task.WhenAll(
            db.StringIncrementAsync("version:CategoryList:global"),
            db.StringIncrementAsync("version:ProductList:global"),
            db.StringIncrementAsync("version:VariantList:global"),
            db.StringIncrementAsync($"version:ProductList:cat:{notification.CategoryId.Value}"),
            db.StringIncrementAsync($"version:VariantList:cat:{notification.CategoryId.Value}")
        );

        var server = _redis.GetServer(_redis.GetEndPoints().First());

        // Delete all product and variant responses that belong to this category
        var productResponseKeys = server.Keys(pattern: "ProductResponse:*").ToArray();
        var productVariantKeys = server.Keys(pattern: "ProductVariantResponse:*").ToArray();
        var keysToDelete = new List<RedisKey>();

        foreach (var key in productResponseKeys)
        {
            var value = await db.StringGetAsync(key);
            if (value.HasValue && value.ToString().Contains(notification.CategoryId.Value.ToString()))
            {
                keysToDelete.Add(key);
            }
        }

        foreach (var key in productVariantKeys)
        {
            var value = await db.StringGetAsync(key);
            if (value.HasValue && value.ToString().Contains(notification.CategoryId.Value.ToString()))
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


