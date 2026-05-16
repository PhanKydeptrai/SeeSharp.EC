using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.CircuitBreaker;
using Polly.Fallback;
using Polly.Timeout;
using StackExchange.Redis;

namespace Infrastructure.Options;

public enum ResilienceKeys
{
    RedisCache,
    ExternalHttp,
    DatabaseQueries
}

public static class CacheServiceExtensions
{

    public static IServiceCollection AddSystemResilience(this IServiceCollection services)
    {
        services.AddResiliencePipeline<string, string?>("redis-get-pipeline", pipelineBuilder =>
        {
            // LỚP 1 (NGOÀI CÙNG): FALLBACK - Trả về null khi có bất kỳ lỗi nào phía dưới dội lên
            pipelineBuilder.AddFallback(new FallbackStrategyOptions<string?>
            {
                ShouldHandle = new PredicateBuilder<string?>()
                    .Handle<RedisConnectionException>()
                    .Handle<RedisTimeoutException>()
                    .Handle<BrokenCircuitException>()
                    .Handle<TimeoutRejectedException>(), // Thêm lỗi do lớp Timeout ở trong cùng quăng ra

                FallbackAction = args => Outcome.FromResultAsValueTask<string?>(null),

                OnFallback = args =>
                {
                    // Log ở mức Debug hoặc Trace để tránh rác log khi hệ thống bị tải cao
                    // Console.WriteLine("[Fallback] Trả về null an toàn.");
                    return default;
                }
            });

            // LỚP 2: CIRCUIT BREAKER - Ngắt mạch nếu Redis phản hồi lỗi hoặc timeout liên tục
            pipelineBuilder.AddCircuitBreaker(new CircuitBreakerStrategyOptions<string?>
            {
                ShouldHandle = new PredicateBuilder<string?>()
                    .Handle<RedisConnectionException>()
                    .Handle<RedisTimeoutException>()
                    .Handle<TimeoutRejectedException>(), // Tính cả những request bị Polly Timeout vào tỉ lệ lỗi

                MinimumThroughput = 10,          // Nên để cao hơn một chút (ví dụ 10) để tránh ngắt mạch do vài lỗi mạng lác đác
                SamplingDuration = TimeSpan.FromSeconds(30),
                FailureRatio = 0.5,              // Nếu 50% trong số 10 request trên bị lỗi
                BreakDuration = TimeSpan.FromSeconds(20), // Tăng thời gian ngắt lên 20-30s cho Redis đủ thời gian hồi phục hoàn toàn

                OnOpened = args =>
                {
                    Console.WriteLine($"[Ngắt mạch] Redis có vấn đề! Tạm ngắt kết nối trong {args.BreakDuration.TotalSeconds}s.");
                    return default;
                },
                OnHalfOpened = args =>
                {
                    Console.WriteLine("[Ngắt mạch] Thử nghiệm lại kết nối Redis...");
                    return default;
                }
            });

            // LỚP 3 (TRONG CÙNG): TIMEOUT BẢO VỆ (Quan trọng khi bỏ Retry)
            // Nếu Redis không lỗi, nhưng phản hồi quá chậm (treo), Polly sẽ cắt ngang sau 200ms
            pipelineBuilder.AddTimeout(TimeSpan.FromMilliseconds(200));
        }); 


        return services;
    }
}