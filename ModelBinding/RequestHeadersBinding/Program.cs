using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/api", ([FromHeader(Name ="X-Api-Version")] string apiVersion) => $"Api Version : {apiVersion}");

app.Run();
