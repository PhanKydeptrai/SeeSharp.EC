using API.Endpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace API.Extentions;

public static class EndpointExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        ServiceDescriptor[] serviceDescriptors = assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                           type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);

        return services;
    }

    public static IApplicationBuilder MapEndpoints(this WebApplication app)
    {
        IEnumerable<IEndpoint> endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        foreach (IEndpoint endpoint in endpoints)
        {
            endpoint.MapEndpoint(app);
        }

        return app;
    }

    /// <summary>
    /// Thêm phản hồi 403 cho các endpoint
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static RouteHandlerBuilder AddForbiddenResponse(
        this RouteHandlerBuilder builder)
    {
        return builder
            .Produces<ProblemDetails>(403, "application/problem+json")
            .WithOpenApi(o =>
            {
                o.Responses["403"].Description = "Không đủ quyền truy cập";
                return o;
            });
    }

    /// <summary>
    /// Thêm phản hồi 401 cho các endpoint
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static RouteHandlerBuilder AddUnauthorizedResponse(
       this RouteHandlerBuilder builder)
    {
        return builder
            .Produces<ProblemDetails>(401, "application/problem+json")
            .WithOpenApi(o =>
            {
                o.Responses["401"].Description = "Chưa xác thực";
                return o;
            });
    }

    /// <summary>
    /// Thêm phản hồi 400 cho các endpoint
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static RouteHandlerBuilder AddBadRequestResponse(
       this RouteHandlerBuilder builder)
    {
        return builder
            .Produces<ProblemDetails>(400, "application/problem+json")
            .WithOpenApi(o =>
            {
                o.Responses["400"].Description = "Yêu cầu không hợp lệ";
                return o;
            });
    }

    /// <summary>
    /// Thêm phản hồi 404 cho các endpoint
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static RouteHandlerBuilder AddNotFoundResponse(
       this RouteHandlerBuilder builder)
    {
        return builder
            .Produces<ProblemDetails>(404, "application/problem+json")
            .WithOpenApi(o =>
            {
                o.Responses["404"].Description = "Không tìm thấy";
                return o;
            });
    }

}
