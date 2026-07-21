using Microsoft.AspNetCore.Mvc;

namespace _02_ResultFilters.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new[] {"Keyboard , 30.15 $","Mouse , 40.25 $"});
    }
}