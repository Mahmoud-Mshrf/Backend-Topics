using _01_DefaultBehaviour.Endpoints;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
}
else
{
    app.UseExceptionHandler("/error-development");
}
app.MapGet("/", () => "Hello World!");
app.MapControllers();
app.MapErrorEndpoints();
app.Run();
