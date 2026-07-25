using AuthenticationWithJWT.Requests;
using AuthenticationWithJWT.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

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
}