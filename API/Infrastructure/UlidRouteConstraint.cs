namespace API.Infrastructure;

public class UlidRouteConstraint : IRouteConstraint
{
    public bool Match(
        HttpContext? httpContext, 
        IRouter? route, 
        string routeKey, 
        RouteValueDictionary values, 
        RouteDirection routeDirection)
    {
        if (values.TryGetValue(routeKey, out var value) && value is string ulidValue)
        {
            return Ulid.TryParse(ulidValue, out _);
        }
        return false;
    }
}
