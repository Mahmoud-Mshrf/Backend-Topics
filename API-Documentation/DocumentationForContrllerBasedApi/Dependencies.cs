using System.Text;
using Asp.Versioning;
using DocumentationForContrllerBasedApi.Data;
using DocumentationForContrllerBasedApi.Helpers;
using DocumentationForContrllerBasedApi.Models;
using DocumentationForContrllerBasedApi.OpenApi.Transformers;
using DocumentationForContrllerBasedApi.Services.Implementations;
using DocumentationForContrllerBasedApi.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DocumentationForContrllerBasedApi;

public static class Dependencies
{
    public static IServiceCollection AddDependencies(this IServiceCollection services , IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.Configure<JwtOptions>(configuration.GetSection("JWT_Options"));
        var jwtOptions = configuration.GetSection("JWT_Options").Get<JwtOptions>();
        services.AddAuthentication(authOptions =>
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

        services.AddAuthorization(options =>
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
        services.AddDbContext<AppDbContext>(x=>x.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IJwtTokenProvider,JwtTokenProvider>();
        services.AddScoped<IAuthService,AuthService>();
        services.AddScoped<IPasswordHasher<AppUser>,PasswordHasher<AppUser>>();
        // services.AddApiVersioning(options =>
        // {
        //     options.ApiVersionReader = ApiVersionReader.Combine(
        //         new MediaTypeApiVersionReader("v"),
        //         new QueryStringApiVersionReader("api-version"));
        //     options.ReportApiVersions=true;
        //     options.DefaultApiVersion= new ApiVersion(1,0);
        //     options.AssumeDefaultVersionWhenUnspecified=true;
        // });
        services
        .AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader= new QueryStringApiVersionReader("api-version");
        })
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = false;
        });
        string[] versions = ["v1", "v2"];

        foreach (var version in versions)
        {
            services.AddOpenApi(version, options =>
            {
               // Versioning config
                options.AddDocumentTransformer<VersionInfoTransformer>();     
               // Security Scheme config
                options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
                options.AddOperationTransformer<BearerSecuritySchemeTransformer>();
                options.AddOperationTransformer<ApiVersionDefaultTransformer>();
            });
        }
        
        return services;
    }
}