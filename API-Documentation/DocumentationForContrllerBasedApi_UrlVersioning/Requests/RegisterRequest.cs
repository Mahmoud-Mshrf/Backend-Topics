using System.ComponentModel.DataAnnotations;

namespace DocumentationForContrllerBasedApi_UrlVersioning.Requests;

public class RegisterRequest
{
    [Required]
    [Length(10,40,ErrorMessage ="User first name must be between 10 and 40 characters")]
    public string? FirstName {get;set;}
    [Required]
    [Length(10,40,ErrorMessage ="User last name must be between 10 and 40 characters")]
    public string? LastName{get;set;}
    [Required]
    [EmailAddress]
    public string? Email {get;set;}
    [Required]
    public string? Password {get;set;}
    [Required]
    public DateOnly BirthDate {get;set;}
}