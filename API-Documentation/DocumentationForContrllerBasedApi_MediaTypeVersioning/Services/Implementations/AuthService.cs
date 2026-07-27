using System.Security.Cryptography;
using DocumentationForContrllerBasedApi.Responses;
using DocumentationForContrllerBasedApi_MediaTypeVersioning.Data;
using DocumentationForContrllerBasedApi_MediaTypeVersioning.Models;
using DocumentationForContrllerBasedApi_MediaTypeVersioning.Requests;
using DocumentationForContrllerBasedApi_MediaTypeVersioning.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DocumentationForContrllerBasedApi_MediaTypeVersioning.Services.Implementations;

public class AuthService(IPasswordHasher<AppUser> hasher,AppDbContext context,IJwtTokenProvider jwtTokenProvider):IAuthService
{
    public async Task<ResultDto> RegisterAsync(RegisterRequest request)
    {
        var existed =await context.Users.AnyAsync(x=>x.Email==request.Email);
        if (existed)
        {
            return new ResultDto
            {
                Message = "Email is already registered before , try to login , or use another email for new registration"
            };
        }
        var user = new AppUser
        {
            FirstName = request.FirstName!,
            LastName = request.LastName!,
            BirthDate = request.BirthDate,
            Email=request.Email!,
        };
        user.PasswordHash = hasher.HashPassword(user,request.Password!);
        await context.AddAsync(user);
        await context.SaveChangesAsync();

        return new ResultDto
        {
            Success =true,
            Message = "User registered successfully go to login"
        };
    }

    public async Task<AuthResult> GetTokenAsync(LoginCredentials credentials)
    {
        var user =await context.Users.FirstOrDefaultAsync(x=>x.Email==credentials.Email);
        if (user == null)
        {
            return null;
        }
        var passwordVerificationResult = hasher.VerifyHashedPassword(user,user.PasswordHash!,credentials.Password!);
        if (passwordVerificationResult!= PasswordVerificationResult.Success)
        {
            return null;
        }

        var authResult = new AuthResult();
        var refreshTokens = user.RefreshTokens;
        if (refreshTokens.Any(x=>x.IsActive))
        {
            var refreshToken = refreshTokens.First(x=>x.IsActive);
            authResult.RefreshToken = refreshToken.Token;
            authResult.RefreshTokenExpiration = refreshToken.ExpiresOn;
        }
        else
        {
            var refreshToken = GenerateRefreshToken();
            authResult.RefreshToken = refreshToken.Token;
            authResult.RefreshTokenExpiration = refreshToken.ExpiresOn;
            user.RefreshTokens.Add(refreshToken);
            await context.SaveChangesAsync();
        }
        var tokenRequest = new TokenRequest
        {
            Id=user.Id,
            BirthDate = user.BirthDate,
            Email=user.Email!,
            FirstName=user.FirstName!,
            LastName=user.LastName!,
            Permissions=user.Permissions?? new List<string>(),
            Roles=user.Roles?? new List<string>(),
        };
        var accessToken = await jwtTokenProvider.GenerateTokenAsync(tokenRequest);
        authResult.AccessToken = accessToken.Token;
        authResult.AccessTokenExpiration= accessToken.Expires;
        
        return authResult;
    }
    
    public async Task<AuthResult> RefreshTokenAsync(string token)
    {
        var user = await context.Users.FirstOrDefaultAsync(x=>x.RefreshTokens.Any(r=>r.Token==token));
        if (user == null)
        {
            return null;            
        }
        var refreshToken = user.RefreshTokens.First(x=>x.Token==token);
        if (!refreshToken.IsActive)
        {
            return null;
        }
        refreshToken.RevokedOn= DateTime.UtcNow;
        var authResult = new AuthResult();
        var newRefreshToken = GenerateRefreshToken();
        authResult.RefreshToken = newRefreshToken.Token;
        authResult.RefreshTokenExpiration = newRefreshToken.ExpiresOn;
        user.RefreshTokens.Add(newRefreshToken);
        await context.SaveChangesAsync();

        var tokenRequest = new TokenRequest
        {
            Id=user.Id,
            BirthDate = user.BirthDate,
            Email=user.Email!,
            FirstName=user.FirstName!,
            LastName=user.LastName!,
            Permissions=user.Permissions?? new List<string>(),
            Roles=user.Roles?? new List<string>(),
        };
        var accessToken = await jwtTokenProvider.GenerateTokenAsync(tokenRequest);
        authResult.AccessToken = accessToken.Token;
        authResult.AccessTokenExpiration= accessToken.Expires;    

        return authResult;
    }
    
    public async Task<bool> RevokeTokenAsync(string token)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.RefreshTokens.Any(x => x.Token == token));
        if (user==null)
        {
            return false;
        }
        var refreshToken = user.RefreshTokens.Single(x => x.Token == token);
        if (!refreshToken.IsActive)
            return false;

        refreshToken.RevokedOn = DateTime.UtcNow;
        await context.SaveChangesAsync();
        return true;
    }
    
    private RefreshToken GenerateRefreshToken()
    {
        string token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        return new RefreshToken
        {
            Token = token,
            CreatedOn = DateTime.UtcNow,
            ExpiresOn = DateTime.UtcNow.AddHours(12)
        };
    }


}



