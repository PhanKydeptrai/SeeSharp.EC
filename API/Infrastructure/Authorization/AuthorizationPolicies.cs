using Microsoft.AspNetCore.Authorization;

namespace API.Infrastructure.Authorization;

public static class AuthorizationPolicies
{
    //ROLE:
    private const string Admin = "Admin";
    private const string Employee = "Employee";
    private const string Subscribed = "Subscribed";
    public const string Guest = "Guest";
    // public const string Customer = "Customer";

    //POLICY:
    public const string AdminOnly = "AdminOnly";
    public const string SubscribedOnly = "SubscribedOnly";
    public const string SubscribedOrGuest = "SubscribedOrGuest";
    public const string AllEmployee = "AllEmployee";

    public static void ConfigurePolicies(AuthorizationOptions options)
    {
        // Admin policy - chỉ admin mới có quyền truy cập
        options.AddPolicy(AdminOnly, policy => 
            policy.RequireRole(Admin));

        // Employee policy - employee và admin đều có quyền truy cập
        options.AddPolicy(AllEmployee, policy => 
            policy.RequireRole(Employee, Admin));

        // Customer policy - customer đều có quyền truy cập
        options.AddPolicy(SubscribedOnly, policy => 
            policy.RequireRole(Subscribed));
        
        // Guest policy - chỉ guest mới có quyền truy cập
        options.AddPolicy(Guest, policy => 
            policy.RequireRole(Guest));
        
        // Subscribed or Guest policy - customer hoặc guest đều có quyền truy cập
        options.AddPolicy(SubscribedOrGuest, policy => 
            policy.RequireRole(Subscribed, Guest));
    }
}