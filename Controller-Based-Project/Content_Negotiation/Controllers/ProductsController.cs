using Content_Negotiation.Data;
using Content_Negotiation.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Content_Negotiation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(ProductRepository repository) : ControllerBase
{
    [HttpGet("{productId:Guid}")]
    [Produces("application/json",["application/xml"])]
    public ActionResult<ProductResponse> GetProduct(Guid productId)
    {
        var product = repository.GetProductById(productId);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(ProductResponse.FromModel(product));
    }
}
