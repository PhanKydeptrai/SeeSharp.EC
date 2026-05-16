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
        // Khi một product mới được tạo, chúng ta cần invalidate cache của danh sách product.
        // CacheKeyGenerator sử dụng versioning với key pattern: "version:product:global" (hoặc theo category).
        // Tăng version sẽ làm cho tất cả các cache key cũ (dựa trên version cũ) bị bỏ qua.

        string globalVersionKey = "version:ProductList:global";
        await _redisDb.StringIncrementAsync(globalVersionKey);
        
        // Nếu bạn muốn invalidate cả cache theo category của product này, 
        // bạn có thể lấy categoryId từ product (cần inject thêm repository hoặc lấy từ event).
        // Tuy nhiên, tăng global version là cách an toàn và bao quát nhất.
    }
}
