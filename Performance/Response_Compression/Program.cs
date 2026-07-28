using System.IO.Compression;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps=true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();

    options.MimeTypes = new[]
    {
        "application/json",
        "text/plain",
        "text/html",
        "application/xml"
    };
});

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level= CompressionLevel.Fastest;
});
builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level= CompressionLevel.Fastest;
});
var app = builder.Build();
app.UseResponseCompression();
app.MapControllers();
app.MapGet("/", () => "Hello World!");

app.Run();
