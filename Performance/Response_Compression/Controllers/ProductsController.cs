using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Response_Compression.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetProducts()
    {
        var products = Enumerable.Range(1, 1000)
            .Select(i => new ProductDto
            {
                Id = i,
                Name = $"Product {i}",
                Category = $"Category {i % 10}",
                Price = Math.Round(10 + (i * 1.25m), 2),
                Description = GenerateLargeText(),
                Tags =
                [
                    "electronics",
                    "gaming",
                    "office",
                    "wireless",
                    "premium",
                    "high-performance",
                    "portable",
                    "accessory"
                ]
            });

        return Ok(products);
    }

    private static string GenerateLargeText()
    {
        var builder = new StringBuilder();

        for (int i = 0; i < 100; i++)
        {
            builder.Append("Lorem ipsum dolor sit amet, consectetur adipiscing elit. ");
            builder.Append("Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. ");
            builder.Append("Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. ");
            builder.Append("Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. ");
            builder.Append("Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. ");
        }

        return builder.ToString();
    }
}

public class ProductDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string Description { get; init; } = string.Empty;
    public List<string> Tags { get; init; } = [];
}