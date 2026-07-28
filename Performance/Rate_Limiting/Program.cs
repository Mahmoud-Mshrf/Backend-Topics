using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("FixedPolicy", limiterOptions =>
    {// in this policy , there are 100 request allowed and after this there is 10 request in the queue and the the oldest requests in the queue processed first
        limiterOptions.Window= TimeSpan.FromMinutes(1);
        limiterOptions.PermitLimit = 100;
        limiterOptions.QueueLimit= 10;
        limiterOptions.QueueProcessingOrder= QueueProcessingOrder.OldestFirst;
    });
    // sliding limiter is better than fixed limiter 
    options.AddSlidingWindowLimiter("SlidingPolicy", limiterOptions =>
    {
        limiterOptions.Window= TimeSpan.FromMinutes(1);
        limiterOptions.PermitLimit=100;
        limiterOptions.QueueLimit=10;
        limiterOptions.QueueProcessingOrder =QueueProcessingOrder.OldestFirst;
        limiterOptions.SegmentsPerWindow=6;
        limiterOptions.AutoReplenishment=true;
    });

    // this limiter specifies the number of allowed request to run in concurrency 
    options.AddConcurrencyLimiter("ConcurrencyPolicy", limiterOptions =>
    {
        limiterOptions.PermitLimit=50;
        limiterOptions.QueueLimit=100;
        limiterOptions.QueueProcessingOrder=QueueProcessingOrder.OldestFirst;
    });

    options.AddPolicy("ByUserPolicy", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey:context.User.Identity?.Name??"anonymous",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                Window=TimeSpan.FromMinutes(1),
                PermitLimit=1000,
                AutoReplenishment=true            
            }
        ));
    options.AddPolicy("ByIpPolicy",HttpContext =>
        RateLimitPartition.GetSlidingWindowLimiter(
            partitionKey:HttpContext.Connection.RemoteIpAddress?.ToString()??"unknown",
            factory:_=> new SlidingWindowRateLimiterOptions
            {
                Window=TimeSpan.FromMinutes(1),
                PermitLimit=100,
                SegmentsPerWindow=6,
                AutoReplenishment=true
            }
        ));
});
var app = builder.Build();
app.UseRateLimiter();
app.MapControllers();
app.MapGet("/", () => "Hello World!").RequireRateLimiting("FixedPolicy");

app.Run();
