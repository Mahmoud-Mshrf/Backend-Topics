using System;
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
// one parameter
app.MapGet("/products/{id}", (int id) =>
{
   return Results.Ok($"Product {id}"); 
});
// multiple parameters
app.MapGet("/date/{day}-{month}-{year}",(int day,int month,int year) =>
{
   return Results.Ok($"today is {day}/{month}/{year}"); 
});
// default parameter 
app.MapGet("/controller/{controller=Home}",(string controller) =>
{
   return Results.Ok(controller); 
});
// option parameter
app.MapGet("/{id?}",(int? id) =>
{
   return Results.Ok(id is null ? "All Users":$"User {id}"); 
});
// catch alls
app.MapGet("/info/{*information}",(string? information) =>
{
   return Results.Ok(information); 
});
app.Run();
