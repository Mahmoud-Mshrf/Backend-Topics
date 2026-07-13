using Microsoft.AspNetCore.Mvc;

namespace _01_WhatIsRouting.Controllers;

[Route("[controller]")]
[ApiController]
public class ProductsController(IServiceProvider sp): ControllerBase
{
    [HttpGet("all")]
    public IActionResult GetProducts()
    {

        return Ok(new[] {new{ProductName = "Product 1", Price = 200},
                         new{ProductName = "Product 2", Price = 220}});
    }
    [HttpGet("all-endpoints")]
    public IActionResult GetEndpoints()
    {
        var endpoints = sp.GetRequiredService<EndpointDataSource>().Endpoints.Select(x=>x.DisplayName);
        return Ok(endpoints);
    }

}