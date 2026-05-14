using Application.IServices;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SharedKernel.Constants;
using StackExchange.Redis;

namespace Infrastructure.Services.RedisServices;

internal sealed class RedisCacheService : IRedisCacheService
{
    private readonly IDatabase _redisDb;

    public RedisCacheService(
        IDatabase redisDb)
    { 
        _redisDb = redisDb;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var cachedValue = await _redisDb.StringGetAsync(key);

        if (cachedValue.IsNull)
        {
            return default;
        }

        return JsonConvert.DeserializeObject<T>(cachedValue!);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        await _redisDb.StringSetAsync(
            key,
            JsonConvert.SerializeObject(value),
            expiration);
    }

    public async Task<IReadOnlyList<T?>> GetManyAsync<T>(
        IEnumerable<string> keys, 
        CancellationToken cancellationToken = default)
    {
        var redisKeys = keys.Select(k => (RedisKey)k).ToArray();

        var cachedValues = await _redisDb.StringGetAsync(redisKeys);

        var results = new List<T?>();
        foreach (var value in cachedValues)
        {
            if (value.HasValue && !value.IsNullOrEmpty)
            {
                results.Add(JsonConvert.DeserializeObject<T>(value.ToString()));
            }
            else
            {
                results.Add(default);
            }
        }

        return results;
    }

    public async Task SetManyAsync<T>(IDictionary<string, T> values, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        var batch = _redisDb.CreateBatch();
        var tasks = new List<Task>();

        foreach (var kvp in values)
        {
            tasks.Add(batch.StringSetAsync(
                kvp.Key,
                JsonConvert.SerializeObject(kvp.Value),
                expiration));
        }

        batch.Execute();
        await Task.WhenAll(tasks);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _redisDb.KeyDeleteAsync(key);
    }

    public async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default)
    {
        return await _redisDb.KeyExistsAsync(key);
    }
}
