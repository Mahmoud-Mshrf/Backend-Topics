namespace RouteConstrains.Constrains;

public class MonthRouteConstraint : IRouteConstraint
{
    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
    {
        if(!values.TryGetValue(routeKey,out var routeValue))
        {
            return false;
        }

        if(int.TryParse(routeValue?.ToString(),out var value))
            return value >= 1 && 12 >= value;
        
        return false;
    }
}