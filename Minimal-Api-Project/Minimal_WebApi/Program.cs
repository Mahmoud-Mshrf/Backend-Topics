using Microsoft.AspNetCore.Http.HttpResults;
using Minimal_WebApi.Data;
using Minimal_WebApi.Endpoints;
using Minimal_WebApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ProductRepository>();
var app = builder.Build();
app.MapProductsEnpoints();
app.Run();
