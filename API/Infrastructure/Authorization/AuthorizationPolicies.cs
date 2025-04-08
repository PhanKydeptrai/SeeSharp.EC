using Microsoft.AspNetCore.Authorization;

namespace API.Infrastructure.Authorization;

public static class AuthorizationPolicies
{
    public const string Admin = "Admin";
    public const string Employee = "Employee";
    public const string Customer = "Customer";

    public static void ConfigurePolicies(AuthorizationOptions options)
    {
        // Admin policy - chỉ admin mới có quyền truy cập
        options.AddPolicy(Admin, policy => 
            policy.RequireRole(Admin));

        // Employee policy - employee và admin đều có quyền truy cập
        options.AddPolicy(Employee, policy => 
            policy.RequireRole(Employee, Admin));

        // Customer policy - customer, employee và admin đều có quyền truy cập
        options.AddPolicy(Customer, policy => 
            policy.RequireRole(Customer, Employee, Admin));
    }
}