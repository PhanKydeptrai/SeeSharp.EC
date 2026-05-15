using Application.IServices;
using Polly;

namespace Infrastructure.Services.RedisServices;


internal sealed class ResilienceRedisCacheService : IRedisCacheService
{
    private readonly IRedisCacheService _innerCacheService;

    public ResilienceRedisCacheService(
        IRedisCacheService innerCacheService)
    {
        _innerCacheService = innerCacheService;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        return await _innerCacheService.GetAsync<T>(key, cancellationToken);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        await _innerCacheService.SetAsync(key, value, expiration, cancellationToken);
    }

    public async Task<IReadOnlyList<T?>> GetManyAsync<T>(IEnumerable<string> keys, CancellationToken cancellationToken = default)
    {
        return await _innerCacheService.GetManyAsync<T>(keys, cancellationToken);
    }

    public async Task SetManyAsync<T>(IDictionary<string, T> values, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        await _innerCacheService.SetManyAsync(values, expiration, cancellationToken);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _innerCacheService.RemoveAsync(key, cancellationToken);   
    }

    public async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default)
    {
        return await _innerCacheService.ExistsAsync(key, cancellationToken);
    }
}