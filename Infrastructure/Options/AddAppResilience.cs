using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Registry;
using SharedKernel.Constants;
using StackExchange.Redis;

namespace Infrastructure.Options;

public static class CacheServiceExtensions
{
    public static IServiceCollection AddAppResilience(this IServiceCollection services)
    {
        IPolicyRegistry<string?> registry = new PolicyRegistry();
        // Nếu lỗi thì trả về null (Cache Miss)
        var fallbackPolicy = Policy<string?>
            .Handle<Exception>()
            .FallbackAsync((string?)null);

        // Nếu lỗi 3 lần liên tiếp, ngắt mạch 30 giây
        var circuitBreakerPolicy = Policy
            .Handle<RedisConnectionException>()
            .Or<RedisTimeoutException>()
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: 3,
                durationOfBreak: TimeSpan.FromSeconds(30)
            );

        var genericCircuitBreaker = circuitBreakerPolicy.AsAsyncPolicy<string?>();

        var resilienceStrategy = Policy.WrapAsync(fallbackPolicy, genericCircuitBreaker);

        registry.Add(Strategy.RedisStrategy, resilienceStrategy);
        services.AddSingleton<IReadOnlyPolicyRegistry<string?>>(registry);
        return services;
    }
}
