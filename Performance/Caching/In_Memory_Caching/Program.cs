using In_Memory_Caching.Data;
using In_Memory_Caching.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(x=>x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IProductService,ProductService>();
builder.Services.AddMemoryCache(options=>options.SizeLimit=100);
var app = builder.Build();
app.MapControllers();
app.MapGet("/", () => "Hello World!");

app.Run();
