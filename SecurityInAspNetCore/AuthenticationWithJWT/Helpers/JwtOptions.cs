namespace AuthenticationWithJWT.Helpers;

public class JwtOptions
{
    public string Issuer;
    public string Audience;
    public int TokenExpirationInMinutes ;
}