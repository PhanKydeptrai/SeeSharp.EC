using System.Collections.Concurrent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Fallback;
using Polly.Timeout;
using StackExchange.Redis;

namespace Infrastructure.Services.RedisServices;

public interface IRedisPipelineFactory
{
    ResiliencePipeline<T?> GetGetCachePipeline<T>();
    ResiliencePipeline<bool> GetVoidPipeline();
}

public class RedisPipelineFactory : IRedisPipelineFactory
{
    private readonly ConcurrentDictionary<Type, object> _pipelines = new();
    private readonly ILogger<RedisPipelineFactory> _logger;

    public RedisPipelineFactory(ILogger<RedisPipelineFactory> logger) // Inject ILogger
    {
        _logger = logger;
    }

    public ResiliencePipeline<T?> GetGetCachePipeline<T>()
    {
        return (ResiliencePipeline<T?>)_pipelines.GetOrAdd(typeof(T), _ => 
        {
            // Lấy thông số từ appsettings, nếu không có thì dùng mặc định
            var timeoutMs = 10;
            var breakSec = 20;

            return new ResiliencePipelineBuilder<T?>()
                .AddFallback(new FallbackStrategyOptions<T?>
                {
                    ShouldHandle = new PredicateBuilder<T?>()
                        .Handle<RedisConnectionException>()
                        .Handle<RedisTimeoutException>()
                        .Handle<BrokenCircuitException>()
                        .Handle<TimeoutRejectedException>()
                        .Handle<OperationCanceledException>(),
                    FallbackAction = args => Outcome.FromResultAsValueTask<T?>(default)
                })
                .AddCircuitBreaker(new CircuitBreakerStrategyOptions<T?>
                {
                    ShouldHandle = new PredicateBuilder<T?>().Handle<RedisConnectionException>().Handle<RedisTimeoutException>().Handle<TimeoutRejectedException>(),
                    MinimumThroughput = 3,
                    SamplingDuration = TimeSpan.FromSeconds(30),
                    FailureRatio = 0.5,
                    BreakDuration = TimeSpan.FromSeconds(breakSec),
                })
                .AddTimeout(TimeSpan.FromMilliseconds(timeoutMs))
                .Build();
        });
    }

    public ResiliencePipeline<bool> GetVoidPipeline()
    {
        return (ResiliencePipeline<bool>)_pipelines.GetOrAdd(typeof(void), _ =>
        {
            var timeoutMs = 10;
            var breakSec = 20;

            // Dùng ResiliencePipelineBuilder TRƠN (Không có <T>)
            return new ResiliencePipelineBuilder<bool>()
                .AddFallback(new FallbackStrategyOptions<bool>
                {
                    ShouldHandle = new PredicateBuilder<bool>()
                        .Handle<RedisConnectionException>()
                        .Handle<RedisTimeoutException>()
                        .Handle<BrokenCircuitException>()
                        .Handle<TimeoutRejectedException>()
                        .Handle<OperationCanceledException>(),
                    
                    // Cách Polly nói: "Nếu lỗi, hãy coi như đã hoàn thành thành công và không làm gì cả"
                    FallbackAction = args => Outcome.FromResultAsValueTask(true)
                })
                .AddCircuitBreaker(new() // Cấu hình Circuit Breaker trơn
                {
                    ShouldHandle = new PredicateBuilder<bool>().Handle<RedisConnectionException>().Handle<RedisTimeoutException>().Handle<TimeoutRejectedException>(),
                    MinimumThroughput = 3,
                    SamplingDuration = TimeSpan.FromSeconds(30),
                    FailureRatio = 0.5,
                    BreakDuration = TimeSpan.FromSeconds(breakSec),
                })
                .AddTimeout(TimeSpan.FromMilliseconds(timeoutMs))
                .Build(); // Trả về một ResiliencePipeline sạch sẽ
        });
    }
}

