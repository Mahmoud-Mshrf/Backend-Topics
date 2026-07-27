using DocumentationForContrllerBasedApi.Responses;
using DocumentationForContrllerBasedApi_MediaTypeVersioning.Requests;

namespace DocumentationForContrllerBasedApi_MediaTypeVersioning.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResult> GetTokenAsync(LoginCredentials credentials);
    Task<ResultDto> RegisterAsync(RegisterRequest request);
    Task<AuthResult> RefreshTokenAsync(string token);
    Task<bool> RevokeTokenAsync(string token);
}