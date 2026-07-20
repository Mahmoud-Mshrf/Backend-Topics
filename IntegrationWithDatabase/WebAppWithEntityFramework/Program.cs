using Microsoft.EntityFrameworkCore;
using WebAppWithEntityFramework.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source = Products.db");
});
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
