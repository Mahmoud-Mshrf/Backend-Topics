using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace _01_DefaultBehaviour.Controllers;

public class ErrorController : ControllerBase
{
    [Route("/error")]
    public IActionResult Get()
    {
        return new ObjectResult(new
        {
           statusCode = 500,
           Detail = "Internal Server Error" 
        });
    }

    [Route("/error-development")]
    public IActionResult GetDevelopmentError([FromServices] IHostEnvironment environment)
    {
        if (!environment.IsDevelopment())
        {
            return NotFound();
        }
        var exceptionHandlerFeature = HttpContext.Features.Get<ExceptionHandlerFeature>();
        return new ObjectResult(new
        {
           statusCode = 500,
           title = exceptionHandlerFeature!.Error.Message,
           Detail = exceptionHandlerFeature.Error.StackTrace
        });
    }
}