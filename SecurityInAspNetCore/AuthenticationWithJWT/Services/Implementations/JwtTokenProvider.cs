using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthenticationWithJWT.Data;
using AuthenticationWithJWT.Helpers;
using AuthenticationWithJWT.Models;
using AuthenticationWithJWT.Requests;
using AuthenticationWithJWT.Responses;
using AuthenticationWithJWT.Services.Interfaces;
using Azure.Core;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationWithJWT.Services.Implementations;

public class JwtTokenProvider(IOptions<JwtOptions> jwtOptions) : IJwtTokenProvider
{
    public async Task<TokenResponse> GenerateTokenAsync(TokenRequest request)
    {
        var issuer = jwtOptions.Value.Issuer;
        var audience = jwtOptions.Value.Audience;
        var ExpiryMinutes = jwtOptions.Value.TokenExpirationInMinutes;
        var key = jwtOptions.Value.SigningKey;

        List<Claim> claims = new()
        {   
            new Claim(JwtRegisteredClaimNames.Sub,request.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.GivenName,request.FirstName),
            new Claim(JwtRegisteredClaimNames.FamilyName,request.FirstName),
            new Claim(JwtRegisteredClaimNames.Birthdate,request.BirthDate.ToString()),
            new Claim(JwtRegisteredClaimNames.Email,request.Email),
        };

        foreach (var role in request.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role,role));
        }

        foreach (var permission in request.Permissions)
        {
            claims.Add(new Claim("Permission",permission));
        }

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Audience = audience,
            Issuer=issuer,
            Expires=DateTime.UtcNow.AddMinutes(ExpiryMinutes),
            IssuedAt = DateTime.UtcNow,
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),SecurityAlgorithms.HmacSha256)
        };
        var tokenHandler = new JwtSecurityTokenHandler();

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var accessToken =await Task.Run(()=>tokenHandler.WriteToken(securityToken));

        return new TokenResponse()
        {
            AccessToken = accessToken,
            Expires=tokenDescriptor.Expires,
            RefreshToken= "12345678910ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        };
    }
}

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

    public async Task<TokenResponse> GetTokenAsync(LoginCredentials credentials)
    {
        var user =await context.Users.FirstOrDefaultAsync(x=>x.Email==credentials.Email);
        if (user == null)
        {
            return null;
        }
        var passwordVerificationResult = hasher.VerifyHashedPassword(user,user.PasswordHash,credentials.Password);
        if (passwordVerificationResult!= PasswordVerificationResult.Success)
        {
            return null;
        }

        var tokenRequest = new TokenRequest
        {
            Id=user.Id,
            BirthDate = user.BirthDate,
            Email=user.Email,
            FirstName=user.FirstName,
            LastName=user.LastName,
            Permissions=user.Permissions,
            Roles=user.Roles,
        };
        return await jwtTokenProvider.GenerateTokenAsync(tokenRequest);
    }
}

