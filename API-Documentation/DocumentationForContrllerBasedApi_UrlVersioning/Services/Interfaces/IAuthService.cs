using DocumentationForContrllerBasedApi_UrlVersioning.Requests;
using DocumentationForContrllerBasedApi_UrlVersioning.Responses;

namespace DocumentationForContrllerBasedApi_UrlVersioning.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResult> GetTokenAsync(LoginCredentials credentials);
    Task<ResultDto> RegisterAsync(RegisterRequest request);
    Task<AuthResult> RefreshTokenAsync(string token);
    Task<bool> RevokeTokenAsync(string token);
}