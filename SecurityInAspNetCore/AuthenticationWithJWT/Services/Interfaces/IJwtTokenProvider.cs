using AuthenticationWithJWT.Requests;
using AuthenticationWithJWT.Responses;

namespace AuthenticationWithJWT.Services.Interfaces;

public interface IJwtTokenProvider
{
    Task<GeneratedAccessToken> GenerateTokenAsync(TokenRequest request); 
}
