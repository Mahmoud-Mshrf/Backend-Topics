using DocumentationForContrllerBasedApi_HeaderVersioning.Requests;
using DocumentationForContrllerBasedApi_HeaderVersioning.Responses;

namespace DocumentationForContrllerBasedApi_HeaderVersioning.Services.Interfaces;

public interface IJwtTokenProvider
{
    Task<GeneratedAccessToken> GenerateTokenAsync(TokenRequest request); 
}
