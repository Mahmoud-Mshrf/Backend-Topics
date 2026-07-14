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
    [HttpHead]
    public IActionResult GetPreferencesfromHead()
    {
        Response.Cookies.Append("token","123ABC");
        Response.Cookies.Append("refreshToken","refresh-123ABC");
        return Ok();
    }
    [HttpOptions]
    public IActionResult getOptions()
    {
        Response.Headers.Append("Allow",new string[]{"Put ","Get ","Patch ","Delete ","Post "});
        return Ok();
    }
}