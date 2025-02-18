using Application.Abstractions.EventBus;
using Application.IServices;
using Infrastructure.BackgoundJob;
using Infrastructure.Consumers.CategoryMessageConsumer;
using Infrastructure.MessageBroker;
using Infrastructure.Services.CategoryServices;
using MassTransit;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Outbox;
using Quartz;

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
            .AddScoped<OutboxProcessor>() //Đăng ký OutboxProcessor
            .AddEventBus()
            .AddRedisConfig(configuration)
            .AddBackgoundJob()
            .AddMassTransitConfiguration(configuration);

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
        services.AddScoped<CategoryQueryServices>();
        services.AddScoped<ICategoryQueryServices>(provider =>
        {
            var categoryQueryServices = provider.GetRequiredService<CategoryQueryServices>();
            return new CategoryQueryServicesDecorated(categoryQueryServices, provider.GetService<IDistributedCache>()!);
        });
        return services;
    }

    private static IServiceCollection AddMassTransitConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMassTransit(busConfiguration =>
        {
            busConfiguration.SetKebabCaseEndpointNameFormatter();
            //* NOTE: Message Broker in memory
            busConfiguration.UsingInMemory((context, config) =>
            {
                config.ConfigureEndpoints(context);
            });

            //* Đăng ký consumer
            busConfiguration.AddConsumer<CategoryCreatedMessageConsumer>();

            //* FIXME: Config RabbitMQ
            #region Config RabbitMQ
            // busConfiguration.UsingRabbitMq((context, cfg) =>
            // {
            //     MessageBrokerSetting messageBrokerSetting = context.GetRequiredService<MessageBrokerSetting>();
            //     cfg.Host(new Uri(messageBrokerSetting.Host), h =>
            //     {
            //         h.Username(messageBrokerSetting.Username);
            //         h.Password(messageBrokerSetting.Password);
            //     });
            // });
            #endregion

        });

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


    private static IServiceCollection AddBackgoundJob(this IServiceCollection services)
    {
        services.AddQuartz(options =>
        {
            //options.UseMicrosoftDependencyInjectionJobFactory();

            var jobKey_OutboxBackgroundService = JobKey.Create(nameof(OutboxBackgroundService));

            options.AddJob<OutboxBackgroundService>(jobKey_OutboxBackgroundService)
                    .AddTrigger(trigger =>
                        trigger.ForJob(jobKey_OutboxBackgroundService)
                    .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(7)
                    .RepeatForever()));

        });

        services.AddQuartzHostedService();
        return services;
    }
}
