using System;
using RouteConstrains.Constrains;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRouting(options=>
{
   options.ConstraintMap.Add("validMonth",typeof(MonthRouteConstraint));
});
var app = builder.Build();

// 
app.MapGet("/int/{id:int}", (int id) =>
{
   return Results.Ok($"Product {id}"); 
});
// 
app.MapGet("/date/{date:datetime}",(DateTime date) =>
{
   return Results.Ok($"today is {date}"); 
});
// 
app.MapGet("/minLength/{name:minlength(12)}",(string name) =>
{
   return Results.Ok(name);
});
// 
app.MapGet("/range/{name:length(12,20)}",(string name) =>
{
   return Results.Ok(name); 
});
// 
app.MapGet("/decimal/{price:decimal}",(decimal price) =>
{
   return Results.Ok($"the price is : {price}"); 
});
//
app.MapGet("/validmonthwithint/{month:range(1,12)}",(int month) =>
{
   return Results.Ok($"(using int) the month is : {month}"); 
});
//
app.MapGet("/validmonth/{month:validMonth}",(int month) =>
{
   return Results.Ok($"the month is : {month}"); 
});

app.Run();
