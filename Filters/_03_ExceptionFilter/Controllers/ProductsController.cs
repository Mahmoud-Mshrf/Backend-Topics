using Microsoft.AspNetCore.Mvc;

namespace _02_ResultFilters.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    [HttpGet()]
    public IActionResult Get()
    {
        var fileName = "somefile.pdf";

        var path = Path.Combine("C:\\Users\\Mahmoud-PC\\Downloads",fileName);

        if (!System.IO.File.Exists(path))
        {
            throw new ArgumentNullException("File Not Found");
        }
        return PhysicalFile(path,"application/pdf",fileName);
    }
}