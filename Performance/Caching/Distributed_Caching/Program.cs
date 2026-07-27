using Distributed_Caching.Data;
using Distributed_Caching.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName= "MahmoudPc_";
});
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(x=>x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IProductService,ProductService>();
var app = builder.Build();
app.MapControllers();
app.Run();
