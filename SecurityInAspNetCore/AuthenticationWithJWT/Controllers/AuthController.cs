using AuthenticationWithJWT.Requests;
using AuthenticationWithJWT.Responses;
using AuthenticationWithJWT.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace AuthenticationWithJWT.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService):ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(RegisterRequest registerRequest)
    {
        var result =await authService.RegisterAsync(registerRequest);
        return result.Success ? Ok(result.Message): BadRequest(result.Message);
    }

    [HttpPost("login")]
    public async Task<IActionResult> login(LoginCredentials credentials)
    {
        var result =await authService.GetTokenAsync(credentials);
        if (result == null)
        {
            return BadRequest("Wrong Credentials , Email or password may be incorrect");
        }
        return Ok(result);
    }

    [HttpGet("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        var token = HttpContext.Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(token))
        {
            return BadRequest("Invalid Token");            
        }
        var result =await authService.RefreshTokenAsync(token);
        if (result == null)
        {
            return BadRequest("Invalid Token");
        }
        return Ok(result);
    }

    [HttpPost("revoke-token")]
    public async Task<IActionResult> RevokeToken()
    {
        var token = HttpContext.Request.Cookies["RefreshToken"];
        if (!string.IsNullOrEmpty(token))
        {
            await authService.RevokeTokenAsync(token);
            var cookieOptions = new CookieOptions
            {
                Secure =true,
                Expires = DateTime.UtcNow.AddDays(-1),
                HttpOnly=true,
                SameSite=SameSiteMode.None,
                Path ="/"
            };    
            HttpContext.Response.Cookies.Delete("RefreshToken",cookieOptions);
        }
        return NoContent();
    }

    public void SetRefreshTokenInCookies(string token , DateTime expiresOn)
    {
        var cookieOptions = new CookieOptions
        {
            Expires = expiresOn.ToLocalTime(),
            HttpOnly=true,
            Secure=true,
            SameSite=SameSiteMode.None,
            Path="/"
        };
        HttpContext.Response.Cookies.Append("RefreshToken",token,cookieOptions);
    }
}