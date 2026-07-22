var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Registers the ProblemDetails service.
// This enables ASP.NET Core to generate RFC 9457-compliant ProblemDetails
// responses for errors handled by middleware such as ExceptionHandler and
// StatusCodePages.
builder.Services.AddProblemDetails();

var app = builder.Build();

// Catches unhandled exceptions globally.
// If AddProblemDetails() is registered, it returns a ProblemDetails response
// instead of an empty 500 response.
app.UseExceptionHandler();

// Generates responses for HTTP status codes that have no response body
// (e.g. 404 Not Found, 405 Method Not Allowed).
// With AddProblemDetails(), these responses are formatted as ProblemDetails.
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    // Displays the Developer Exception Page for unhandled exceptions.
    // This page contains detailed debugging information and takes precedence
    // over UseExceptionHandler() while running in the Development environment.
    app.UseDeveloperExceptionPage();
}

app.MapControllers();

app.Run();