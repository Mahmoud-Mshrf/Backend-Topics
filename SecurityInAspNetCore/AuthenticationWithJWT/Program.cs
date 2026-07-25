using System.Reflection.Metadata;
using System.Text;
using AuthenticationWithJWT.Data;
using AuthenticationWithJWT.Helpers;
using AuthenticationWithJWT.Models;
using AuthenticationWithJWT.Services.Implementations;
using AuthenticationWithJWT.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensibility;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JWT_Options"));
var jwtOptions = builder.Configuration.GetSection("JWT_Options").Get<JwtOptions>();
builder.Services.AddAuthentication(authOptions =>
{
    authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;// Specifies that JWT Bearer is the default authentication handler used to authenticate incoming requests and create the ClaimsPrincipal.
    // Specifies which authentication handler should respond when an unauthenticated user tries to access a protected resource.
    // For JWT Bearer, that response is typically: Return 401 Unauthorized , Include a WWW-Authenticate: Bearer header
    authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer= true,
        ValidateAudience = true,
        ValidateIssuerSigningKey= true,
        ValidateLifetime = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidAudience = jwtOptions.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey))
    };
});

builder.Services.AddAuthorization(options =>
{
    // Project Management Permissions
    options.AddPolicy(Permission.Project.Create, policy => policy.RequireClaim("permission", Permission.Project.Create));
    options.AddPolicy(Permission.Project.Read, policy => policy.RequireClaim("permission", Permission.Project.Read));
    options.AddPolicy(Permission.Project.Update, policy => policy.RequireClaim("permission", Permission.Project.Update));
    options.AddPolicy(Permission.Project.Delete, policy => policy.RequireClaim("permission", Permission.Project.Delete));
    options.AddPolicy(Permission.Project.AssignMember, policy => policy.RequireClaim("permission", Permission.Project.AssignMember));
    options.AddPolicy(Permission.Project.ManageBudget, policy => policy.RequireClaim("permission", Permission.Project.ManageBudget));

    // Task Management Permissions (demonstrating granularity)
    options.AddPolicy(Permission.Task.Create, policy => policy.RequireClaim("permission", Permission.Task.Create));
    options.AddPolicy(Permission.Task.Read, policy => policy.RequireClaim("permission", Permission.Task.Read));
    options.AddPolicy(Permission.Task.Update, policy => policy.RequireClaim("permission", Permission.Task.Update));
    options.AddPolicy(Permission.Task.Delete, policy => policy.RequireClaim("permission", Permission.Task.Delete));
    options.AddPolicy(Permission.Task.AssignUser, policy => policy.RequireClaim("permission", Permission.Task.AssignUser));
    options.AddPolicy(Permission.Task.UpdateStatus, policy => policy.RequireClaim("permission", Permission.Task.UpdateStatus));
    options.AddPolicy(Permission.Task.Comment, policy => policy.RequireClaim("permission", Permission.Task.Comment));
});
builder.Services.AddDbContext<AppDbContext>(x=>x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IJwtTokenProvider,JwtTokenProvider>();
builder.Services.AddScoped<IAuthService,AuthService>();
builder.Services.AddScoped<IPasswordHasher<AppUser>,PasswordHasher<AppUser>>();
var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/", () => "Hello World!");
app.Run();
