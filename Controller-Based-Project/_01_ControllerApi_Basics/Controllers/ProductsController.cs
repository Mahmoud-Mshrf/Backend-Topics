using System;
using _01_ControllerApi_Basics.Data;
using _01_ControllerApi_Basics.Dtos;
using _01_ControllerApi_Basics.Models;
using Microsoft.AspNetCore.Mvc;

namespace _01_ControllerApi_Basics.Controllers;
// what is ActionResult
// ActionResult in asp .net core :
//  - abstraction that encapsulate http response
//  - Handle status codes , content formatting and headers
//  - represent the result of controller actions
[Route("api/[controller]")]// specify the global route for the controller
[ApiController]// enable model validation using data annotations and return problem details as error response + [FromBody],[FromQuery]and...etc binding
public class ProductsController(ProductRepository repository) : ControllerBase // enable direct dependency injection for services in the constructor 
{
    [HttpOptions]
    public IActionResult OptionsProduct()
    {
        Response.Headers.Append("Allow","Get , Head , Post , Put , Patch , Delete");
        return NoContent();
    }

    [HttpHead("{productId:guid}")]
    public IActionResult HeadProduct(Guid productId)
    {
        return repository.ExistsById(productId)? Ok() : NotFound();
    }

    [HttpGet("{productId:guid}")]
    public ActionResult<ProductResponse> GetProduct(Guid productId,bool includeReviews = false)
    {
        var product = repository.GetProductById(productId);
        if (product == null)
        {
            return NotFound();
        }
        List<ProductReview> reviews = null;
        if (includeReviews)
        {
            reviews = repository.GetProductReviews(productId);
        }
        return Ok(ProductResponse.FromModel(product,reviews));
    }

    [HttpGet]
    public ActionResult<PageResult<ProductResponse>> GetProducts(int page =1 , int size=10)
    {
        page = Math.Max(1,page);
        size = Math.Clamp(size,1,100);
        var result = new PageResult<ProductResponse>
        {
            Items = ProductResponse.FromModels(repository.GetProductsPage(page, size)),
            CurrentPage = page,
            PageSize = size,
            TotalCount = repository.GetProductsCount()
        };
        return Ok(result);
    }
}




