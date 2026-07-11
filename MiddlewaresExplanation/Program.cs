using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

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
app.Run();

