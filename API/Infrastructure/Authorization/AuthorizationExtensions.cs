using API.Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace API.Extentions;

public static class AuthorizationExtensions
{
    /// <summary>
    /// Yêu cầu quyền Admin để truy cập endpoint
    /// </summary>
    public static RouteHandlerBuilder RequireAdmin(this RouteHandlerBuilder builder)
    {
        return builder.RequireAuthorization(AuthorizationPolicies.AdminOnly);
    }

    /// <summary>
    /// Yêu cầu quyền Employee hoặc Admin để truy cập endpoint
    /// </summary>
    public static RouteHandlerBuilder RequireEmployee(this RouteHandlerBuilder builder)
    {
        return builder.RequireAuthorization(AuthorizationPolicies.AllEmployee);
    }

    /// <summary>
    /// Yêu cầu quyền Subscribed để truy cập endpoint
    /// </summary>
    public static RouteHandlerBuilder RequireSubscribed(this RouteHandlerBuilder builder)
    {
        return builder.RequireAuthorization(AuthorizationPolicies.SubscribedOnly);
    }

    /// <summary>
    /// Yêu cầu quyền Guest để truy cập endpoint
    /// </summary>
    public static RouteHandlerBuilder RequireGuest(this RouteHandlerBuilder builder)
    {
        return builder.RequireAuthorization(AuthorizationPolicies.Guest);
    }

    /// <summary>
    /// Yêu cầu quyền Subscribed hoặc Guest để truy cập endpoint
    /// </summary>
    public static RouteHandlerBuilder RequireSubscribedOrGuest(this RouteHandlerBuilder builder)
    {
        return builder.RequireAuthorization(AuthorizationPolicies.SubscribedOrGuest);
    }

    /// <summary>
    /// Áp dụng quyền truy cập theo vai trò được chỉ định
    /// </summary>
    public static RouteHandlerBuilder RequireRoles(this RouteHandlerBuilder builder, params string[] roles)
    {
        return builder.RequireAuthorization(policy => policy.RequireRole(roles));
    }
} 