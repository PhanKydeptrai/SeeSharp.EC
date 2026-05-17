using Application.IServices;
using Polly;

namespace Infrastructure.Services.RedisServices;


internal sealed class ResilienceRedisCacheService : IRedisCacheService
{
    private readonly IRedisCacheService _innerCacheService;
    private readonly IRedisPipelineFactory _pipelineFactory;

    public ResilienceRedisCacheService(
        IRedisCacheService innerCacheService,
        IRedisPipelineFactory pipelineFactory)
    {
        _innerCacheService = innerCacheService;
        _pipelineFactory = pipelineFactory;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var pipeline = _pipelineFactory.GetGetCachePipeline<T>();
        return await pipeline.ExecuteAsync(async ct =>
        {
            return await _innerCacheService.GetAsync<T>(key, ct);
        }, cancellationToken);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        var voidPipeline = _pipelineFactory.GetVoidPipeline();
        await voidPipeline.ExecuteAsync(async ct =>
        {
            await _innerCacheService.SetAsync(key, value, expiration, ct);
            return true; // Giá trị trả về không quan trọng vì chúng ta đã cấu hình fallback để bỏ qua lỗi

        }, cancellationToken);
    }

    public async Task<IReadOnlyList<T?>> GetManyAsync<T>(IEnumerable<string> keys, CancellationToken cancellationToken = default)
    {
        var pipeline = _pipelineFactory.GetGetCachePipeline<IReadOnlyList<T?>>();

        return await pipeline.ExecuteAsync(async ct =>
        {
            return await _innerCacheService.GetManyAsync<T>(keys, ct);
        }, cancellationToken);
    }

    public async Task SetManyAsync<T>(IDictionary<string, T> values, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        var voidPipeline = _pipelineFactory.GetVoidPipeline();
        await voidPipeline.ExecuteAsync(async ct =>
        {
            await _innerCacheService.SetManyAsync(values, expiration, ct);
            return true;
        }, cancellationToken);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        var voidPipeline = _pipelineFactory.GetVoidPipeline();
        await voidPipeline.ExecuteAsync(async ct =>
        {
            await _innerCacheService.RemoveAsync(key, ct);
            return true;
        }, cancellationToken);
    }

    public async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default)
    {

        return await _innerCacheService.ExistsAsync(key, cancellationToken);

    }
}