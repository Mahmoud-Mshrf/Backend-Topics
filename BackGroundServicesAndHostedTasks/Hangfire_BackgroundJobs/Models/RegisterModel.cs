using System.ComponentModel.DataAnnotations;

namespace Hangfire_BackgroundJobs.Models;

public class RegisterModel
{
    [Required]
    public string Email {get;set;}
    [Required]
    public string Password {get;set;}
    [Required]
    public string FirstName {get;set;}
    [Required]
    public string LastName {get;set;}
}