var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(p =>
    {
        p.WithOrigins("https://localhost:7130")
        .AllowAnyMethod();
    });
});
var app = builder.Build();
app.UseCors();
app.MapGet("/", () => "Hello World!");

app.Run();

