namespace API.Infrastructure.Authorization;

public static class AuthorizationRoles
{
    //ROLE:
    public const string Admin = "Admin";
    public const string Employee = "Employee";
    public const string Subscribed = "Subscribed";
    public const string Guest = "Guest";
}
public static class AuthorizationPolicies
{
    //POLICY:
    public const string AdminOnly = "AdminOnly";
    public const string SubscribedOnly = "SubscribedOnly";
    public const string SubscribedOrGuest = "SubscribedOrGuest";
    public const string AllEmployee = "AllEmployee";
}