using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using WebAppWithEntityFramework.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source = Products.db");
});
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
});
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapControllers();
app.Run();
