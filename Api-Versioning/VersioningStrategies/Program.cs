using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using VersioningStrategies.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSingleton<ProductRepository>();
builder.Services.AddApiVersioning(options =>
{
   options.ApiVersionReader= new UrlSegmentApiVersionReader();
   options.AssumeDefaultVersionWhenUnspecified=true;
   options.DefaultApiVersion = new ApiVersion(1,0); 
   options.ReportApiVersions = true;
});
var app = builder.Build();
app.MapControllers();
app.MapGet("/", () => "Hello World!");

app.Run();

