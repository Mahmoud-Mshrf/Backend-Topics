using In_Memory_Caching.Data;
using In_Memory_Caching.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(x=>x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IProductService,ProductService>();
// add redis caching support
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration= builder.Configuration.GetConnectionString("Redis");
    options.InstanceName= "Mahmoud-Pc:";
});
// actually one of redis caching and sql server caching will be use , which last one registered in di container (sql server in this case), but here we show how to configure both .
// add sql server caching support
// builder.Services.AddDistributedSqlServerCache(options =>
// {
//     options.ConnectionString= builder.Configuration.GetConnectionString("SqlServerCaching");
//     options.SchemaName="dbo";
//     options.TableName= "CachingEntry";
// // we need to write the following commands to install dotnet-sql-cache :
// // dotnet tool install --global dotnet-sql-cache  (this to install it)
// // dotnet sql-cache create "Write ConnectionString here" 'scheme name' 'table name' , for example
// // dotnet sql-cache create "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=LocalCachingTestDb;Integrated Security=True;" dbo CachingEntry
// });

// add hybrid caching configuration 
builder.Services.AddHybridCache(options =>
{
    options.DefaultEntryOptions = new HybridCacheEntryOptions
    {
        Expiration= TimeSpan.FromMinutes(15),// this for redis or sql server caching 
        LocalCacheExpiration = TimeSpan.FromMinutes(1)// this for in memory caching (in hybrid caching , in-memory caching included by default , no need to register it manually in di container)
    };
    
});
var app = builder.Build();
app.MapControllers();
app.MapGet("/", () => "Hello World!");

app.Run();
