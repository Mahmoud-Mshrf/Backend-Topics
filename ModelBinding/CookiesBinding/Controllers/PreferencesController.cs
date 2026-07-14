using Microsoft.AspNetCore.Mvc;

namespace CookiesBinding.Controllers;

[Route("[controller]")]
[ApiController]
public class PreferenceSController : ControllerBase
{
    [HttpGet]
    public IActionResult GetPreferences()
    {
        var language = Request.Cookies["language"];
        var theme = Request.Cookies["theme"];
        var timeZone = Request.Cookies["timeZone"];

        return Ok(new
        {
            language,
            theme,
            timeZone
        });
    }
}