namespace Application.IServices;

public interface IRedisCacheService
{
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<T?>> GetManyAsync<T>(IEnumerable<string> keys, CancellationToken cancellationToken = default);
    Task SetManyAsync<T>(IDictionary<string, T> values, TimeSpan? expiration = null, CancellationToken cancellationToken = default);
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default);
}
