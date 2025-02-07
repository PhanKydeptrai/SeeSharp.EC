using Application.Abstractions.EventBus;
using Infrastructure.MessageBroker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    //FIXME: AddInfrastructure
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHealthCheck(configuration)
            .AddServices()
            .AddEventBus()
            .AddRedisConfig(configuration);

        return services;
    }

    private static IServiceCollection AddHealthCheck(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("PrimaryDatabase");
        string? readOnlyConnectionString = configuration.GetConnectionString("ReadOnlyDatabase");
        string? redisConnectionString = configuration.GetConnectionString("Redis");

        //Message Broker
        string? rabbitMQHost = configuration.GetConnectionString("MessageBroker:Host");
        string? ServerName = configuration.GetConnectionString("MessageBroker:Username");

        ////? Thiếu một đống cấu hình HealthCheck
        //services.AddHealthChecks()
        //    .AddCheck<DatabaseHealthCheck>("DatabaseHealthCheck", HealthStatus.Unhealthy)
        //    //.AddCheck<ReadOnlyDatabaseHealthCheck>("DatabaseHealthCheckReadOnly", HealthStatus.Unhealthy)
        //    .AddNpgSql(connectionString!)
        //    .AddDbContextCheck<NextSharpReadOnlyDbContext>()
        //    //.AddRabbitMQ(rabbitMQConnectionString, "RabbitMQ", HealthStatus.Unhealthy)
        //    .AddRedis(redisConnectionString!);

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        //services.AddScoped<CategoryRepository>();
        //services.AddScoped<ICategoryRepository>(provider =>
        //{
        //    var categoryRepository = provider.GetRequiredService<CategoryRepository>();

        //    return new CategoryCachedRepository(categoryRepository, provider.GetService<IDistributedCache>()!);
        //});


        //services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }

    private static IServiceCollection AddEventBus(this IServiceCollection services)
    {
        services.AddTransient<IEventBus, EventBus>();
        return services;
    }

    private static IServiceCollection AddRedisConfig(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string connection = configuration.GetConnectionString("Redis")
                ?? throw new ArgumentNullException("Redis connection string is missing");

        services.AddStackExchangeRedisCache(redisOptions =>
        {
            redisOptions.Configuration = connection;
        });

        return services;
    }
}
