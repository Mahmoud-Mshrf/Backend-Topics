namespace AuthenticationWithJWT.Models;

public class AppUser
{
    public Guid Id {get;set;}
    public string? FirstName {get;set;}
    public string? LastName {get;set;}
    public string? Email {get;set;}
    public DateOnly BirthDate {get;set;}
    public string? PasswordHash {get;set;}
    public List<string>? Roles {get;set;} = [];
    public List<string>? Permissions {get;set;} = [];
    public List<RefreshToken> RefreshTokens {get;set;} = [];
}