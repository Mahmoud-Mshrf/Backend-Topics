using AuthenticationWithJWT.Data;
using AuthenticationWithJWT.Models;
using AuthenticationWithJWT.Requests;
using AuthenticationWithJWT.Responses;
using AuthenticationWithJWT.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationWithJWT.Services.Implementations;

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
        var passwordVerificationResult = hasher.VerifyHashedPassword(user,user.PasswordHash!,credentials.Password!);
        if (passwordVerificationResult!= PasswordVerificationResult.Success)
        {
            return null;
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
        return await jwtTokenProvider.GenerateTokenAsync(tokenRequest);
    }
}

