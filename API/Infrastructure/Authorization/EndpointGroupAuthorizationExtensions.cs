using API.Extentions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace API.Infrastructure.Authorization;

public static class EndpointGroupAuthorizationExtensions
{
    /// <summary>
    /// Tạo một nhóm endpoint với yêu cầu quyền Admin
    /// </summary>
    public static RouteGroupBuilder MapAdminGroup(this IEndpointRouteBuilder app, string prefix, Action<RouteGroupBuilder> configure)
    {
        var group = app.MapGroup(prefix);
        configure(group);
        group.RequireAuthorization(AuthorizationPolicies.AdminOnly);
        return group;
    }

    /// <summary>
    /// Tạo một nhóm endpoint với yêu cầu quyền Employee
    /// </summary>
    public static RouteGroupBuilder MapEmployeeGroup(this IEndpointRouteBuilder app, string prefix, Action<RouteGroupBuilder> configure)
    {
        var group = app.MapGroup(prefix);
        configure(group);
        group.RequireAuthorization(AuthorizationPolicies.AllEmployee);
        return group;
    }

    /// <summary>
    /// Tạo một nhóm endpoint với yêu cầu quyền Subscribed
    /// </summary>
    public static RouteGroupBuilder MapSubscribedGroup(this IEndpointRouteBuilder app, string prefix, Action<RouteGroupBuilder> configure)
    {
        var group = app.MapGroup(prefix);
        configure(group);
        group.RequireAuthorization(AuthorizationPolicies.SubscribedOnly);
        return group;
    }

    /// <summary>
    /// Tạo một nhóm endpoint với yêu cầu quyền Guest
    /// </summary>
    public static RouteGroupBuilder MapGuestGroup(this IEndpointRouteBuilder app, string prefix, Action<RouteGroupBuilder> configure)
    {
        var group = app.MapGroup(prefix);
        configure(group);
        group.RequireAuthorization(AuthorizationPolicies.Guest);
        return group;
    }

    /// <summary>
    /// Tạo một nhóm endpoint với yêu cầu quyền Subscribed hoặc Guest
    /// </summary>
    public static RouteGroupBuilder MapSubscribedOrGuestGroup(this IEndpointRouteBuilder app, string prefix, Action<RouteGroupBuilder> configure)
    {
        var group = app.MapGroup(prefix);
        configure(group);
        group.RequireAuthorization(AuthorizationPolicies.SubscribedOrGuest);
        return group;
    }

    /// <summary>
    /// Tạo một nhóm endpoint với yêu cầu quyền theo vai trò được chỉ định
    /// </summary>
    public static RouteGroupBuilder MapRoleGroup(this IEndpointRouteBuilder app, string prefix, string[] roles, Action<RouteGroupBuilder> configure)
    {
        var group = app.MapGroup(prefix);
        configure(group);
        group.RequireAuthorization(policy => policy.RequireRole(roles));
        return group;
    }
} 