using DocumentationForContrllerBasedApi.Requests;
using DocumentationForContrllerBasedApi.Responses;

namespace DocumentationForContrllerBasedApi.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResult> GetTokenAsync(LoginCredentials credentials);
    Task<ResultDto> RegisterAsync(RegisterRequest request);
    Task<AuthResult> RefreshTokenAsync(string token);
    Task<bool> RevokeTokenAsync(string token);
}