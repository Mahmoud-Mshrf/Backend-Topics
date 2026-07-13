using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
// route parameters has higher precedence than over query strings except in case of you specify it(you specify FromQuery attribute to make query strings have higher precedence)
// app.MapGet("/products/{id:int}", ([FromRoute(Name="id")] int identifier) => $"product {identifier}");
// app.MapGet("/products/{id:int}", ([FromQuery] int identifier) => $"product {identifier}");
app.MapGet("/products", ([FromQuery] int identifier) => $"product {identifier}");

app.Run();
