using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace _03_ExceptionFilter.Filters;

public class GlobalExceptionFilter : IAsyncExceptionFilter
{
    public Task OnExceptionAsync(ExceptionContext context)
    {
        var problem = new ProblemDetails
        {
            Status = 500,
            Title = "Internal Server Error",
            Detail = context.Exception.Message
        };

        context.Result = new ObjectResult(problem)
        {
            StatusCode = problem.Status
        };

        context.ExceptionHandled = true;

        return Task.CompletedTask;
    }
}