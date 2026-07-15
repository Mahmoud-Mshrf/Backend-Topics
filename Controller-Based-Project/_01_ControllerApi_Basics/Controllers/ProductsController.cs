using Microsoft.AspNetCore.Mvc;

namespace _01_ControllerApi_Basics.Controllers;
// what is ActionResult
// ActionResult in asp .net core :
//  - abstraction that encapsulate http response
//  - Handle status codes , content formatting and headers
//  - represent the result of controller actions
[Route("api/[controller]")]// specify the global route for the controller
[ApiController]// enable model validation using data annotations and return problem details as error response + [FromBody],[FromQuery]and...etc binding
public class ProductsController : ControllerBase // enable direct dependency injection for services in the constructor 
{
    [HttpOptions]
    public IActionResult OptionsProduct()
    {
        Response.Headers.Append("Allow","Get , Head , Post , Put , Patch , Delete");
        return NoContent();
    }

}


