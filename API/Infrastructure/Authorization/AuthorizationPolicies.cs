using Microsoft.AspNetCore.Authorization;

namespace API.Infrastructure.Authorization;

public static class AuthorizationPolicies
{
    //ROLE:
    public const string Admin = "Admin";
    public const string Employee = "Employee";
    public const string Subscribed = "Subscribed";
    // public const string Customer = "Customer";

    //POLICY:
    public const string AdminOnly = "AdminOnly";
    public const string SubscribedOnly = "SubscribedOnly";

    public static void ConfigurePolicies(AuthorizationOptions options)
    {
        // Admin policy - chỉ admin mới có quyền truy cập
        options.AddPolicy(AdminOnly, policy => 
            policy.RequireRole(Admin));

        // Employee policy - employee và admin đều có quyền truy cập
        options.AddPolicy(Employee, policy => 
            policy.RequireRole(Employee, Admin));

        // Customer policy - customer đều có quyền truy cập
        options.AddPolicy(SubscribedOnly, policy => 
            policy.RequireRole(Subscribed));
    }
}