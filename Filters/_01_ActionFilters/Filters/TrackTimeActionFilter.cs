using Microsoft.AspNetCore.Mvc.Filters;

namespace _01_ActionFilters.Filters;

public class TrackTimeActionFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        context.HttpContext.Items["ActionStartedAt"] = DateTime.UtcNow;
        await next();
        var startTime =(DateTime) context.HttpContext.Items["ActionStartedAt"]!;
        var elapsedTime =DateTime.UtcNow - startTime;
        context.HttpContext.Response.Headers.Append("ElapsedTime",$"{elapsedTime.Microseconds} ms");
    }
}
public class TrackTimeActionFilterV2 : Attribute,IAsyncActionFilter
// public class TrackTimeActionFilterV2 : ActionFilterAttribute // this line and the above line are the same
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        context.HttpContext.Items["ActionStartedAt"] = DateTime.UtcNow;
        await next();
        var startTime =(DateTime) context.HttpContext.Items["ActionStartedAt"]!;
        var elapsedTime =DateTime.UtcNow - startTime;
        context.HttpContext.Response.Headers.Append("ElapsedTime",$"{elapsedTime.Microseconds} ms");
    }
}
// public class TrackTimeActionFilter : IActionFilter
// {
//     public void OnActionExecuted(ActionExecutedContext context)
//     {
//         throw new NotImplementedException();
//     }

//     public void OnActionExecuting(ActionExecutingContext context)
//     {
//         throw new NotImplementedException();
//     }
// }