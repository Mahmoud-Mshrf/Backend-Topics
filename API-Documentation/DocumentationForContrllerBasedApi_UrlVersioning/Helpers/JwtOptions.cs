namespace DocumentationForContrllerBasedApi_UrlVersioning.Helpers;

public class JwtOptions
{
    public string? Issuer {get;set;}
    public string? Audience {get;set;}
    public string? SigningKey {get;set;}
    public int TokenExpirationInMinutes {get;set;}
}