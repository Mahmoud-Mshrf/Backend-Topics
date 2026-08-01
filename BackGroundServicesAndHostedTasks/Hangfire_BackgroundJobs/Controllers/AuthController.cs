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
        // fire and forget job
        BackgroundJob.Enqueue(()=>SendEmail(registerRequest.Email)); // or 
        // scheduled job
        BackgroundJob.Schedule(()=>SendEmail(registerRequest.Email),TimeSpan.FromSeconds(2)); // or 
        // minutely job
        RecurringJob.AddOrUpdate("minutely-email",()=>SendEmail(registerRequest.Email),Cron.Minutely);// every minute email
        // daily job
        RecurringJob.AddOrUpdate("daily-email",()=>SendEmail(registerRequest.Email),Cron.Daily);// every day email

        // Sometimes a second job depends on the first one , Instead of running both independently, Hangfire lets you create a dependency.
        var jobId = BackgroundJob.Enqueue(() => GeneratePdf());
        BackgroundJob.ContinueJobWith(
            jobId,
            () => SendEmail(registerRequest.Email));
        return NoContent();
    }

    public void GeneratePdf()
    {
        System.Console.WriteLine("pdf generated");
    }

    public async Task SendEmail(string userEmail)
    {
        await Task.Delay(2000);
        System.Console.WriteLine($"Email has been sent to {userEmail} at {DateTimeOffset.UtcNow}");
    }
}