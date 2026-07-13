using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/generateactivationurl/{userid:int}",(int userid,LinkGenerator linkGenerator,HttpContext context) =>
{
    var url = linkGenerator.GetUriByName("activate-account",new{userid},context.Request.Scheme,context.Request.Host);
    return Results.Ok($"Click here to activate account : {url}");
});
app.MapGet("/activate/{userid:int}",(int userid) =>
{
    return Results.Ok($"Account activated for user : {userid}");
}).WithName("activate-account");
app.Run();
