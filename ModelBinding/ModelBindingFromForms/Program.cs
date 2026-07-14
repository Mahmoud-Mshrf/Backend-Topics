using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
// 
app.MapGet("/fromform", ([FromForm] string name,[FromForm] int age) =>
{
    return Results.Ok($"{name} is {age} years old");
}).DisableAntiforgery();

app.MapPost("/upload", async (IFormFile file) =>
{
    if(file is null || file.Length==0)
        return Results.BadRequest("no file uploaded");

    var uploads = Path.Combine(Directory.GetCurrentDirectory(),"uploads");
    Directory.CreateDirectory(uploads);
    var filePath = Path.Combine(uploads,file.FileName);
    
    using var stream = new FileStream(filePath,FileMode.Create);
    await file.CopyToAsync(stream);

    return Results.Ok("File uploaded");
}).DisableAntiforgery();
app.Run();
