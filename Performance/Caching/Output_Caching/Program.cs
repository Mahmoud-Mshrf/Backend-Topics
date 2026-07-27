using Microsoft.EntityFrameworkCore;
using Output_Caching.Data;
using Output_Caching.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddOutputCache(options =>
{
    // here if we want to override the default options or settings
    // options.DefaultExpirationTimeSpan = TimeSpan.FromMinutes(10);
    // options.MaximumBodySize = 64 * 1024; // it means 64 kb
    // options.SizeLimit = 100 * 1024 * 1024; // it means 100 mb
    // options.UseCaseSensitivePaths = false;

    // here we are able to add policies
    options.AddPolicy("Single-Product", builder =>
    {
        builder.SetVaryByRouteValue(["productId"]).Expire(TimeSpan.FromSeconds(10));
        builder.Tag("products");
    });
});
var app = builder.Build();

app.UseOutputCache();
app.MapControllers();
app.MapGet("/api/products/mini-get",async (IProductService productService, int page =1, int size=10) =>
{
    System.Console.WriteLine("minimal-endpoint visited");
    var result =await productService.GetProductsAsync(page,size);
    return Results.Ok(result);
}).CacheOutput(options =>
{
    options.Expire(TimeSpan.FromSeconds(10))
    .SetVaryByQuery(["page","size"]);
});

app.MapGet("/api/products/mini-get/{productId:int}",async (int productId, IProductService productService) =>
{
    System.Console.WriteLine("minimal-endpoint visited");
    var result =await productService.GetProductByIdAsync(productId);
    return result is  not null ? Results.Ok(result) : 
    Results.NotFound($"product with id {productId} not found");
}).CacheOutput("Single-Product");
// .CacheOutput(options => options.SetVaryByRouteValue(["productId"]).Expire(TimeSpan.FromSeconds(10)));
app.Run();