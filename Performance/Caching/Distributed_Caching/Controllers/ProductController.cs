using Distributed_Caching.Requests;
using Distributed_Caching.Responses;
using Distributed_Caching.Services;
using Microsoft.AspNetCore.Mvc;

namespace Distributed_Caching.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponse>>> Get()
    {
        var response = await productService.GetProductsAsync();
        return Ok(response);
    }

    [HttpGet("{productId:int}", Name = nameof(GetById))]
    public async Task<ActionResult<ProductResponse>> GetById(int productId)
    {
        var response = await productService.GetProductByIdAsync(productId);
        if (response is null)
            return NotFound($"Product with Id '{productId}' not found");

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateProductRequest request)
    {
        var response = await productService.AddProductAsync(request);
        return CreatedAtRoute(nameof(GetById), new { productId = response.ProductId }, response);
    }

    [HttpPut("{productId:int}")]
    public async Task<IActionResult> Put(int productId, [FromBody] UpdateProductRequest request)
    {
        await productService.UpdateProductAsync(productId, request);
        return NoContent();
    }

    [HttpDelete("{productId:int}")]
    public async Task<IActionResult> Delete(int productId)
    {
        await productService.DeleteProductAsync(productId);
        return NoContent();
    }
}