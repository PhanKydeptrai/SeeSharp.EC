using Application.Abstractions.Behaviors;
using FluentValidation;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;
//FIXME: AddApplication
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly); //NOTE: Chưa hiểu
            //Đăng ký logging pipeline behavior
            config.AddOpenBehavior(typeof(RequestLoggingPipelineBehavior<,>));
            //Đăng ký validation pipeline behavior
            config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
        });
        //NOTE: Chưa hiểu
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly, includeInternalTypes: true);
        //Cấu hình MassTransit
        services.AddMassTransitConfiguration(configuration);
        //Cấu hình FluentValidation
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
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
            //busConfiguration.AddConsumer<CategoryCreatedEventConsumer>();

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
}
