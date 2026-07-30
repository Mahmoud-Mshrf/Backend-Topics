using System.Text.Json.Serialization;
using M01.RepositoryPattern.Data;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using PaymentServiceApi.Exceptions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = (context) =>
    {
        context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
        context.ProblemDetails.Extensions.Add("requestId", context.HttpContext.TraceIdentifier);
    };
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source = app.db");
});
// ****** Observability ******
// builder.Services.AddSerilog();//or
// 
builder.Host.UseSerilog((context, LoggerConfiguration) =>
{
    LoggerConfiguration.ReadFrom.Configuration(builder.Configuration);
});
// opentelemetry for distributed tracing configurations
builder.Services.AddOpenTelemetry().ConfigureResource(res=>res.AddService("orderservice"))
.WithTracing(tracing =>
{
    tracing.AddAspNetCoreInstrumentation();
    tracing.AddHttpClientInstrumentation();

    tracing.AddOtlpExporter();
})
.WithMetrics(metrics =>
{
    metrics.AddAspNetCoreInstrumentation();
    metrics.AddHttpClientInstrumentation();

    metrics.AddOtlpExporter()
    .AddPrometheusExporter();
});

var app = builder.Build();

app.UseExceptionHandler();

app.UseStatusCodePages();
// observability
app.UseOpenTelemetryPrometheusScrapingEndpoint();
app.UseSerilogRequestLogging();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

app.Run();
