using Application.Abstractions.Behaviors;
using FluentValidation;
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
        //Cấu hình FluentValidation
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
        return services;
    }

}
