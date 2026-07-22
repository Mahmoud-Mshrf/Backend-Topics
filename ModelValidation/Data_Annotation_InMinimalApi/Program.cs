using System.Text.Json.Serialization;
using Data_Annotation_InMinimalApi.Requests;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<JsonOptions>(options =>
{
   options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()); 
});
var app = builder.Build();

app.MapPost("/api/products", (CreateProductRequest  request) =>
{
    return Results.Created($"/api/products/{Guid.NewGuid()}",request);
}).Validate<CreateProductRequest>();
app.Run();
