using Microsoft.AspNetCore.Http.HttpResults;
using Minimal_WebApi.Data;
using Minimal_WebApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ProductRepository>();
var app = builder.Build();

app.MapGet("/text", () => "Hello World!");

app.MapGet("/json", () =>
{
    return new { Name = "Product from json"}; 
});

app.MapGet("/products/i-result/{id:guid}", (System.Guid id , ProductRepository repository) =>
{
   var product = repository.GetProductById(id);

   return product == null   
           ? Results.NotFound()
           : Results.Ok(product);
});

app.MapGet("/products/type-result/{id:guid}", Results<Ok<Product>,NotFound> (System.Guid id , ProductRepository repository) =>
{
   var product = repository.GetProductById(id);

   return product == null   
           ? TypedResults.NotFound()
           : TypedResults.Ok(product);
});
app.Run();
