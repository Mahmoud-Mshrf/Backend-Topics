using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)// register services required by authentication services
.AddCookie();// add cookie authentication and it add CookieAuthenticationDefaults.AuthenticationScheme as Authentication scheme internally even you don't specify it

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.UseAuthentication();// enable authentication so its responsible for getting user info from cookies or headers ... and create the User in the httpContext 

app.MapGet("/login",async context =>
{
   var claims = new List<Claim>
   {
       new ("Name","Mahmoud"),
       new ("Sub",Guid.NewGuid().ToString()),
       new ("Email","Mahmoud@localhost")
   }; 

   ClaimsIdentity identity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);

   var principal = new ClaimsPrincipal(identity);

    await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,principal);
});

app.MapGet("/user", (HttpContext context) =>
{
    var principal = context.User;
    if (principal.Identity is {IsAuthenticated:true})
    {
        var claims = principal.Claims.Select(x=>new {x.Type , x.Value});
        return Results.Ok(claims);
    }
    return Results.Unauthorized();
});


app.MapGet("/logout",async (HttpContext context) =>
{
    await context.SignOutAsync();
});

app.Run();



