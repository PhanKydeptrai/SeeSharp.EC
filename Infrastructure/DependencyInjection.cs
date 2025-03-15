using Application.Abstractions.EventBus;
using Application.IServices;
using Application.Security;
using Infrastructure.BackgoundJob;
using Infrastructure.Consumers.CategoryMessageConsumer;
using Infrastructure.Consumers.CustomerMessageConsumer;
using Infrastructure.Consumers.OrderMessageConsumer;
using Infrastructure.Consumers.ProductMessageConsumer;
using Infrastructure.Consumers.WishListMessageConsumer;
using Infrastructure.MessageBroker;
using Infrastructure.Security;
using Infrastructure.Services;
using Infrastructure.Services.CategoryServices;
using Infrastructure.Services.CustomerServices;
using Infrastructure.Services.OrderServices;
using Infrastructure.Services.ProductServices;
using Infrastructure.Services.WishItemServices;
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
            .AddMassTransitConfiguration();

        // Cấu hình FluentEmail
        services.AddScoped<EmailVerificationLinkFactory>();
        //Mail Test
        services.AddFluentEmail(configuration["Email:SenderEmail"], configuration["Email:Sender"])
                .AddSmtpSender(configuration["Email:Host"], int.Parse(configuration["Email:Port"]!));
                
        //Mail Thật
        // services.AddFluentEmail(configuration["Email:SenderEmail"], configuration["Email:Sender"])
        //          .AddSmtpSender(new SmtpClient(configuration["Email:Host"], int.Parse(configuration["Email:Port"])));

        //Add TokenProvider
        services.AddScoped<ITokenProvider, TokenProvider>();
        services.AddScoped<ITokenRevocationService, TokenRevocationService>();
        services.AddHttpContextAccessor();
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
        //NOTE: provider bị lỏ
        services.AddScoped<CategoryQueryServices>();
        services.AddScoped<ICategoryQueryServices>(provider =>
        {
            var categoryQueryServices = provider.GetRequiredService<CategoryQueryServices>();
            return new CategoryQueryServicesDecorated(categoryQueryServices, provider.GetService<IDistributedCache>()!);
        });

        services.AddScoped<ProductQueryServices>();
        services.AddScoped<IProductQueryServices>(provider =>
        {
            var productQueryServices = provider.GetRequiredService<ProductQueryServices>();
            return new ProductQueryServicesDecorated(productQueryServices, provider.GetService<IDistributedCache>()!);
        });

        services.AddScoped<ICustomerQueryServices, CustomerQueryServices>();
        services.AddScoped<IOrderQueryServices, OrderQueryServices>();
        services.AddScoped<IWishItemQueryServices, WishItemQueryServices>();
        return services;
    }

    private static IServiceCollection AddMassTransitConfiguration(
        this IServiceCollection services)
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
            busConfiguration.AddConsumer<CategoryUpdatedMessageConsumer>();
            busConfiguration.AddConsumer<CategoryDeletedMessageConsumer>();
            busConfiguration.AddConsumer<ProductCreatedMessageConsumer>();
            busConfiguration.AddConsumer<ProductUpdatedMessageConsumer>();
            busConfiguration.AddConsumer<ProductDeletedMessageConsumer>();
            busConfiguration.AddConsumer<ProductRestoredMessageConsumer>();
            busConfiguration.AddConsumer<CustomerSignedUpMessageConsumer>();
            busConfiguration.AddConsumer<AccountVerificationEmailSentMessageConsumer>();
            busConfiguration.AddConsumer<CustomerVerifiedEmailMessageConsumer>();
            busConfiguration.AddConsumer<CategoryRestoredMessageConsumer>();
            busConfiguration.AddConsumer<CustomerChangePasswordMessageConsumer>();
            busConfiguration.AddConsumer<CustomerConfirmChangePasswordMessageConsumer>();
            busConfiguration.AddConsumer<CustomerChangePasswordSuccessNotificationMessageConsumer>();
            busConfiguration.AddConsumer<CustomerSignedUpWithGoogleAccountMessageConsumer>();    
            busConfiguration.AddConsumer<CustomerResetPasswordEmailSendMessageConsumer>();  
            busConfiguration.AddConsumer<CustomerResetPasswordMessageConsumer>();
            busConfiguration.AddConsumer<CustomerResetPasswordSuccessNotificationMessageConsumer>();
            busConfiguration.AddConsumer<CustomerAddProductToOrderMessageConsumer>();
            busConfiguration.AddConsumer<CustomerUpdateOrderDetailMessageConsumer>();
            busConfiguration.AddConsumer<CustomerDeleteOrderDetailMessageConsumer>();
            busConfiguration.AddConsumer<AddWishItemMessageConsumer>();
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
                    .WithSimpleSchedule(schedule => schedule.WithIntervalInMinutes(5)
                    .RepeatForever()));

        });

        services.AddQuartzHostedService();
        return services;
    }
}
