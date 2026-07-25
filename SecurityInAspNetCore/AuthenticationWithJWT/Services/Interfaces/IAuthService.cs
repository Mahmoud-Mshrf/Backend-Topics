using AuthenticationWithJWT.Requests;
using AuthenticationWithJWT.Responses;
using AuthenticationWithJWT.Services.Implementations;

namespace AuthenticationWithJWT.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResult> GetTokenAsync(LoginCredentials credentials);
    Task<ResultDto> RegisterAsync(RegisterRequest request);
}