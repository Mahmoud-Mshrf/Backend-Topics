using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Rate_Limiting.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private static readonly List<ProductDto> Products =
    [
        new(1, "Laptop", 1200),
        new(2, "Keyboard", 80),
        new(3, "Mouse", 35)
    ];

    [HttpGet]
    [EnableRateLimiting("FixedPolicy")]
    public IActionResult GetAll()
    {
        return Ok(Products);
    }
    
    [HttpGet("{id:int}")]
    [EnableRateLimiting("SlidingPolicy")]
    public IActionResult GetById(int id)
    {
        var product = Products.FirstOrDefault(p => p.Id == id);

        if (product is null)
            return NotFound(new { Message = $"Product with id {id} was not found." });

        return Ok(product);
    }

    [HttpPost]
    [EnableRateLimiting("ConcurrencyPolicy")]
    public IActionResult Create(CreateProductRequest request)
    {
        var product = new ProductDto(
            Products.Max(p => p.Id) + 1,
            request.Name,
            request.Price);

        Products.Add(product);

        return CreatedAtAction(
            nameof(GetById),
            new { id = product.Id },
            product);
    }

    [HttpPut("{id:int}")]
    [EnableRateLimiting("ByUserPolicy")]
    public IActionResult Update(int id, UpdateProductRequest request)
    {
        var index = Products.FindIndex(p => p.Id == id);

        if (index == -1)
            return NotFound(new { Message = $"Product with id {id} was not found." });

        Products[index] = new ProductDto(id, request.Name, request.Price);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [EnableRateLimiting("ByIpPolicy")]
    public IActionResult Delete(int id)
    {
        var product = Products.FirstOrDefault(p => p.Id == id);

        if (product is null)
            return NotFound(new { Message = $"Product with id {id} was not found." });

        Products.Remove(product);

        return NoContent();
    }
}

public record ProductDto(
    int Id,
    string Name,
    decimal Price);

public record CreateProductRequest(
    string Name,
    decimal Price);

public record UpdateProductRequest(
    string Name,
    decimal Price);