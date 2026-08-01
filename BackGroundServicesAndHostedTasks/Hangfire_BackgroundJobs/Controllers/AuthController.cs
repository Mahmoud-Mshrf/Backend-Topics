using Hangfire;
using Hangfire_BackgroundJobs.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hangfire_BackgroundJobs.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterModel registerRequest)
    {
        await Task.Run(() =>
        {
            Task.Delay(2000);
        });
        BackgroundJob.Enqueue(()=>SendEmail(registerRequest.Email));
        return NoContent();
    }
    public async Task SendEmail(string userEmail)
    {
        await Task.Delay(2000);
        System.Console.WriteLine($"Email has been sent to {userEmail} at {DateTimeOffset.UtcNow}");
    }
}