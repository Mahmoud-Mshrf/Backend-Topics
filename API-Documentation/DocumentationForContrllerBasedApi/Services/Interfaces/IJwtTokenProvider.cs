using DocumentationForContrllerBasedApi.Requests;
using DocumentationForContrllerBasedApi.Responses;

namespace DocumentationForContrllerBasedApi.Services.Interfaces;

public interface IJwtTokenProvider
{
    Task<GeneratedAccessToken> GenerateTokenAsync(TokenRequest request); 
}
