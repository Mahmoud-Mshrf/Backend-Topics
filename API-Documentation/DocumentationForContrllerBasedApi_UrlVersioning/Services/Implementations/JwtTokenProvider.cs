using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Azure.Core;
using DocumentationForContrllerBasedApi_UrlVersioning.Helpers;
using DocumentationForContrllerBasedApi_UrlVersioning.Requests;
using DocumentationForContrllerBasedApi_UrlVersioning.Responses;
using DocumentationForContrllerBasedApi_UrlVersioning.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DocumentationForContrllerBasedApi_UrlVersioning.Services.Implementations;

public class JwtTokenProvider(IOptions<JwtOptions> jwtOptions) : IJwtTokenProvider
{
    public async Task<GeneratedAccessToken> GenerateTokenAsync(TokenRequest request)
    {
        var issuer = jwtOptions.Value.Issuer;
        var audience = jwtOptions.Value.Audience;
        var ExpiryMinutes = jwtOptions.Value.TokenExpirationInMinutes;
        var key = jwtOptions.Value.SigningKey;

        List<Claim> claims = new()
        {   
            new Claim(JwtRegisteredClaimNames.Sub,request.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.GivenName,request.LastName),
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
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!)),SecurityAlgorithms.HmacSha256)
        };
        var tokenHandler = new JwtSecurityTokenHandler();

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var accessToken =await Task.Run(()=>tokenHandler.WriteToken(securityToken));

        return new GeneratedAccessToken()
        {
            Token = accessToken,
            Expires=tokenDescriptor.Expires,
        };
    }
}

