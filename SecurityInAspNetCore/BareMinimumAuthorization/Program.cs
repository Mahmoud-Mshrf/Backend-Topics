using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)// register services required by authentication services
.AddCookie();// add cookie authentication and it add CookieAuthenticationDefaults.AuthenticationScheme as Authentication scheme internally even you don't specify it
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admins-only", builder =>
    {
        builder.RequireAssertion(x =>
        {
            // if(x.User.Claims.Any(x=>x.Type==ClaimTypes.Role && x.Value == "Admin"))
            // {
            //     return true;
            // }
            // return false;
            // or
            return x.User.IsInRole("Admin"); 
        });
    });
    options.AddPolicy("GreaterThan25", builder =>
    {
        builder.RequireAssertion(context =>
        {
            // if ((!context.User.Identity?.IsAuthenticated) ?? true)// if the user is not authenticated or if the identity is null
            // {
            //     return false;
            // }
            // var ageClaim = context.User.FindFirst("Age");
            // var ageExisted = int.TryParse(ageClaim?.Value,out int age);
            // if (ageExisted)
            // {
            //     return age >= 25;
            // }
            // return false;
            // or 
            if(context.User.Claims.Any(x=>x.Type=="Age" && 25 <= Convert.ToInt32(x.Value)))
            {
                return true;
            }
            return false;
        });
    });
});
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.UseAuthentication();// enable authentication so its responsible for getting user info from cookies or headers ... and create the User in the httpContext 
app.UseAuthorization();
app.MapGet("/login",async context =>
{
   var claims = new List<Claim>
   {
       new ("Name","Mahmoud"),
       new ("Sub",Guid.NewGuid().ToString()),
       new ("Email","Mahmoud@localhost"),
       new("Age","26"),
       new(ClaimTypes.Role,"Admin"),
       new(ClaimTypes.Role,"Supervisor")
   }; 

   ClaimsIdentity identity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);

   var principal = new ClaimsPrincipal(identity);

    await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,principal);
});

app.MapGet("/secure",(HttpContext context) =>
{
     var claims =context.User.Claims.Select(x=>new {x.Type,x.Value});
     return Results.Ok(claims);
}).RequireAuthorization("GreaterThan25");


app.MapGet("/manage-employees",(HttpContext context) =>
{
     var claims =context.User.Claims.Select(x=>new {x.Type,x.Value});
     return Results.Ok(claims);
}).RequireAuthorization(x=>x.RequireRole("Supervisor"));


app.MapGet("/manage-all",(HttpContext context) =>
{
     var claims =context.User.Claims.Select(x=>new {x.Type,x.Value});
     return Results.Ok(claims);
}).RequireAuthorization("Admins-only");

app.MapGet("/logout",async (HttpContext context) =>
{
    await context.SignOutAsync();
});

app.MapGet("/account/login",async (HttpContext context) =>
{
    return "Login Page";
});

app.Run();



