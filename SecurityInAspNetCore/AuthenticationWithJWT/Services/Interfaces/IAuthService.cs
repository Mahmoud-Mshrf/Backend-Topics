using AuthenticationWithJWT.Requests;
using AuthenticationWithJWT.Responses;

namespace AuthenticationWithJWT.Services.Interfaces;

public interface IAuthService
{
    Task<TokenResponse> GetTokenAsync(LoginCredentials credentials);
    Task<ResultDto> RegisterAsync(RegisterRequest request);
}