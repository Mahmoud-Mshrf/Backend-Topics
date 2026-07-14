using Microsoft.AspNetCore.Mvc;

namespace _01_ControllerApi_Basics.Controllers;

[Route("api/[controller]")]// specify the global route for the controller
[ApiController]// enable model validation using data annotations and return problem details as error response + [FromBody],[FromQuery]and...etc binding
public class ProductsController : ControllerBase // enable direct dependency injection for services in the constructor 
{
    [HttpGet]// maps get request to api/products
    public IActionResult Get()
    {
        return Ok("product 1");
    }

}
// ActionResult in asp .net core :
//  - abstraction that encapsulate http response
//  - Handle status codes , content formatting and headers
//  - represent the result of controller actions