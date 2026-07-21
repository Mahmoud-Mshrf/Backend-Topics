using _01_ActionFilters.Filters;
using Microsoft.AspNetCore.Mvc;

namespace _01_ActionFilters.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    [TrackTimeActionFilterV2]
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new[] {"Keyboard , 30.15 $","Mouse , 40.25 $"});
    }
}