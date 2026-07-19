using System.Net.Mime;
using System;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using VersioningStrategies.Data;
using VersioningStrategies.Dtos;
using VersioningStrategies.Models;
using VersioningStrategies.Dtos.V1;

namespace VersioningStrategies.Controllers.V1;
[ApiVersion("1.0")]
[Route("api/[controller]")]// specify the global route for the controller
[ApiController]// enable model validation using data annotations and return problem details as error response + [FromBody],[FromQuery]and...etc binding
[Route("api/v{version:apiVersion}/[controller]")]
public class ProductsController(ProductRepository repository) : ControllerBase // enable direct dependency injection for services in the constructor 
{

    [HttpGet("{productId:guid}")]
    public ActionResult<ProductResponse> GetProductById(Guid productId,bool includeReviews = false)
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




