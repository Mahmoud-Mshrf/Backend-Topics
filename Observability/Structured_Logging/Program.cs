using Microsoft.Extensions.Logging.Console;
using Structured_Logging.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddScoped<OrderService>();
builder.Logging.ClearProviders();// disable all logging providers (console - eventlog - debug)
builder.Logging.AddConsole(); // enable console logging
// if you don't specify logging configurations in appsettings.json , here we configure them (in code)
// builder.Logging.SetMinimumLevel(LogLevel.Information);
// builder.Logging.AddFilter("Microsoft",LogLevel.Warning);
// builder.Logging.AddFilter("Microsoft.Hosting.Lifetime",LogLevel.Information);

// builder.Logging.AddFilter<ConsoleLoggerProvider>((category,level) =>
// {
//     if (category != null && category.StartsWith("Microsoft"))
//     {
//         return level >= LogLevel.Information;
//     }
    
//     if (category != null && category.StartsWith("Structured_Logging.Services"))
//     {
//         return level >= LogLevel.Warning;
//     }
//     return level >= LogLevel.Error;
// });

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapControllers();
app.Run();
