using DocumentationForContrllerBasedApi_HeaderVersioning.Requests;
using DocumentationForContrllerBasedApi_HeaderVersioning.Responses;

namespace DocumentationForContrllerBasedApi_HeaderVersioning.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResult> GetTokenAsync(LoginCredentials credentials);
    Task<ResultDto> RegisterAsync(RegisterRequest request);
    Task<AuthResult> RefreshTokenAsync(string token);
    Task<bool> RevokeTokenAsync(string token);
}