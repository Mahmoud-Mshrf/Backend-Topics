using System.Security.AccessControl;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();
app.UseHttpsRedirection();
app.MapControllers();

app.MapGet("/all", () =>
{
   return Results.Ok(new[]{new{ProductName ="Product 1",Price = 100},
                           new{ProductName ="Product 2",Price = 120}}); 
} 
);

app.Run();
