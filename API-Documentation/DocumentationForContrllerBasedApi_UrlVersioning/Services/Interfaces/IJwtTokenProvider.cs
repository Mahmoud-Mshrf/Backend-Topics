using DocumentationForContrllerBasedApi_UrlVersioning.Requests;
using DocumentationForContrllerBasedApi_UrlVersioning.Responses;

namespace DocumentationForContrllerBasedApi_UrlVersioning.Services.Interfaces;

public interface IJwtTokenProvider
{
    Task<GeneratedAccessToken> GenerateTokenAsync(TokenRequest request); 
}
