using System.Security.Cryptography;
using System.Text;
using Infrastructure.Helper;
using Polly;
using StackExchange.Redis;

namespace Infrastructure.Services;

internal class CacheKeyGenerator : ICacheKeyGenerator
{
    private readonly IDatabase _redisDb;
    private readonly IAsyncPolicy<string> _redisResiliencePolicy;

    public CacheKeyGenerator(IConnectionMultiplexer redis)
    {
        _redisDb = redis.GetDatabase();

        // Setup Polly: Timeout 500ms và Fallback trả về "1" nếu Redis gặp sự cố
        _redisResiliencePolicy = Policy<string>
            .Handle<Exception>()
            .FallbackAsync("1") // Nếu lỗi, ngầm định version là v1
            .WrapAsync(Policy.TimeoutAsync(TimeSpan.FromMilliseconds(500), Polly.Timeout.TimeoutStrategy.Pessimistic));
    }

    public async Task<string> CreateCacheKeyAsync(
        string listPrefix,
        string? filterProductStatus,
        string? filterCategory,
        string? sortColumn,
        string? sortOrder,
        int? page,
        int? pageSize,
        string? filterProduct = null)
    {
        string scope = string.IsNullOrWhiteSpace(filterCategory) ? "global" : $"cat:{filterCategory.Trim().ToLower()}";
        string versionKey = $"version:{listPrefix}:{scope}";

        string version = await _redisResiliencePolicy.ExecuteAsync(async () =>
        {
            var val = await _redisDb.StringGetAsync(versionKey);
            return val.HasValue ? val.ToString() : "1";
        });

        return BuildHashKey(
            listPrefix, version, filterProductStatus, filterCategory, 
            sortColumn, sortOrder, page, pageSize, filterProduct);

    }
    private string BuildHashKey(
        string listPrefix,
        string version,
        string? filterProductStatus,
        string? filterCategory,
        string? sortColumn,
        string? sortOrder,
        int? page,
        int? pageSize,
        string? filterProduct)
    {
        // 1. Gom chuỗi
        var sb = new StringBuilder();

        if (!string.IsNullOrWhiteSpace(filterCategory))
            sb.Append("cat=").Append(filterCategory.Trim().ToLower()).Append('|');

        if (!string.IsNullOrWhiteSpace(filterProduct))
            sb.Append("prod=").Append(filterProduct.Trim().ToLower()).Append('|');

        if (!string.IsNullOrWhiteSpace(filterProductStatus))
            sb.Append("stat=").Append(filterProductStatus.Trim().ToLower()).Append('|');

        if (!string.IsNullOrWhiteSpace(sortColumn))
            sb.Append("sort=").Append(sortColumn.Trim().ToLower()).Append('|');

        if (!string.IsNullOrWhiteSpace(sortOrder))
            sb.Append("ord=").Append(sortOrder.Trim().ToLower()).Append('|');

        sb.Append("p=").Append(page ?? 1).Append('|');
        sb.Append("sz=").Append(pageSize ?? 10);

        string rawString = sb.ToString();

        // 2. Tối ưu bộ nhớ bằng Span và StackAlloc
        int byteCount = Encoding.UTF8.GetByteCount(rawString);

        // Cấp phát byte trên Stack (Thay vì cấp phát mảng byte[] trên Heap)
        Span<byte> stringBytes = byteCount <= 256 ? stackalloc byte[byteCount] : new byte[byteCount];
        Encoding.UTF8.GetBytes(rawString, stringBytes);

        Span<byte> hashBytes = stackalloc byte[16]; // MD5 luôn là 16 bytes
        MD5.HashData(stringBytes, hashBytes);

        string hashHex = Convert.ToHexString(hashBytes).ToLower();

        // 3. Lắp ráp kết quả
        return $"{listPrefix}:v{version}:{hashHex}";
    }
}
