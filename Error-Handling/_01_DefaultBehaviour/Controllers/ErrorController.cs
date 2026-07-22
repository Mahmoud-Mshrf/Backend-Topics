using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace _01_DefaultBehaviour.Controllers;

public class ErrorController : ControllerBase
{
    [Route("/error")]
    public IActionResult Get()
    {
        // return new ObjectResult(new
        // {
        //    statusCode = 500,
        //    Detail = "Internal Server Error" 
        // });

        // support problem details (manually) :
        var problem = new ProblemDetails
        {
            Detail = "Unexpected error happened",
            Instance = HttpContext.Request.Path,
            Status = StatusCodes.Status500InternalServerError,
            Title = "Internal Server Error",
            Type = "https://example.com/probs/internal-server-error"
        };

        return new ObjectResult(problem)
        {
            StatusCode = problem.Status
        };

    }

    [Route("/error-development")]
    public IActionResult GetDevelopmentError([FromServices] IHostEnvironment environment)
    {
        if (!environment.IsDevelopment())
        {
            return NotFound();
        }
        var exception = HttpContext.Features.Get<ExceptionHandlerFeature>()!.Error;

        var problem = new ProblemDetails
        {
            Detail = exception.StackTrace,
            Instance = HttpContext.Request.Path,
            Status = StatusCodes.Status500InternalServerError,
            Title = exception.Message??"An unexpected error",
            Type = "https://example.com/probs/internal-server-error"
        };
        return new ObjectResult(problem)
        {
            StatusCode = problem.Status
        };
    }
}