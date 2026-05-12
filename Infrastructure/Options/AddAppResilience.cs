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
            .FallbackAsync((string?)null); // Nuốt lỗi

        // Nếu lỗi 3 lần, ngắt mạch 30 giây
        var circuitBreakerPolicy = Policy
            .Handle<RedisConnectionException>()
            .Or<RedisTimeoutException>()
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: 3,
                durationOfBreak: TimeSpan.FromSeconds(30)
            );

        var genericCircuitBreaker = circuitBreakerPolicy.AsAsyncPolicy<string?>();

        var cachePolicy = fallbackPolicy.WrapAsync(genericCircuitBreaker);
        registry.Add(Strategy.RedisStrategy, cachePolicy); // cachePolicy cho Redis
        services.AddSingleton<IReadOnlyPolicyRegistry<string?>>(registry);
        return services;
    }
}
