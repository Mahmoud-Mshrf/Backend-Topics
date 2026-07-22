using Data_Annotation_Validation.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Data_Annotation_Validation.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    [HttpPost]
    public IActionResult CreateProduct(CreateProductRequest request)
    {
        return  Created($"/api/products/{Guid.NewGuid()}",request);
    }
}