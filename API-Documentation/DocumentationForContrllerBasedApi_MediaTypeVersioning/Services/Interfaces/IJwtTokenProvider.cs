using DocumentationForContrllerBasedApi.Responses;
using DocumentationForContrllerBasedApi_MediaTypeVersioning.Requests;

namespace DocumentationForContrllerBasedApi_MediaTypeVersioning.Services.Interfaces;

public interface IJwtTokenProvider
{
    Task<GeneratedAccessToken> GenerateTokenAsync(TokenRequest request); 
}
