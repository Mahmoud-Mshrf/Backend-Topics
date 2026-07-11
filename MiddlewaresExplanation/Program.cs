using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
// middleware app.use 
app.Use((RequestDelegate next) =>
{
    return async (HttpContext context) =>
    {
        var sw = new Stopwatch();
        sw.Start();
        await next(context);
        sw.Stop();
        System.Console.WriteLine(
            $"Request takes {sw.ElapsedMilliseconds} ms from middleware 1");
    };
});;

app.Use(async(HttpContext context, RequestDelegate next) =>
{
   await context.Response.WriteAsJsonAsync("from middleware 2 ");
   await next(context); 
});

// middleware app.run (is a terminal middleware , handle the request and doesn't call the next() , ends the pipeline , nothing after it will run)
app.Run(async(HttpContext context) =>
{
   await context.Response.WriteAsJsonAsync("terminal middleware , ends of pipeline ");
});
app.Run();

