using GlobalErrorHandling.ErrorHandling;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddExceptionHandler<GlobalErrorHandler>();
builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = ( context) =>
    {
        context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
        context.ProblemDetails.Extensions.Add("requestId","");  
    };
        
});
var app = builder.Build();
app.UseExceptionHandler();
app.UseStatusCodePages();
app.MapGet("/", () => "Hello World!");
app.MapControllers();
app.Run();


/*
| Scenario                                                       | Result                                             |
| -------------------------------------------------------------- | -------------------------------------------------- |
| Minimal API + `Results.NotFound()`                             | 404 only                                           |
| Minimal API + `AddProblemDetails()` + `UseStatusCodePages()`   | 404 + ProblemDetails                               |
| Controller + `[ApiController]` + `return NotFound()`           | 404 + ProblemDetails                               |
| Controller **without** `[ApiController]` + `return NotFound()` | 404 only (even if `UseStatusCodePages()` is added) |

*/
