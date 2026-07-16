using System;
using System.Reflection.Metadata.Ecma335;
using _01_ControllerApi_Basics.Data;
using _01_ControllerApi_Basics.Dtos;
using _01_ControllerApi_Basics.Models;
using Microsoft.AspNetCore.JsonPatch;
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

    [HttpGet("{productId:guid}",Name = "GetProductById")]
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
    [HttpPost]
    public IActionResult CreateProduct(CreateProductRequest request)
    {
        if(repository.ExistsByName(request.Name))
            return Conflict($"A product with the name {request.Name} is already exists");

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name=request.Name,
            Price=request.Price
        };
        repository.AddProduct(product);

        return CreatedAtRoute(routeName: nameof(GetProductById),
        routeValues: new {productId = product.Id}
        ,value: ProductResponse.FromModel(product));

    }

    [HttpPut("{productId:guid}")]
    public IActionResult UpdateProduct(Guid productId,UpdateProductRequest request)
    {
        var product = repository.GetProductById(productId);
        if (product==null)
        {
            return NotFound($"there is no product with id : {productId}");
        }
        product.Name = request.Name;
        product.Price = request.Price?? 0;

        var succeeded = repository.UpdateProduct(product);
        if (!succeeded)
        {
            return StatusCode(500,"failed to update product");
        }
        return NoContent();
    }
    [HttpPatch("{productId:guid}")]
    public IActionResult UpdateProductPrice(Guid productId,JsonPatchDocument<UpdateProductRequest> patchDocument)
    {
        if(patchDocument == null)
        {
            return BadRequest("patch document can't be null");
        }
        var product = repository.GetProductById(productId);
        if (product == null)
        {
            return NotFound("product not found");
        }

        var patchModel = new UpdateProductRequest
        {
            Name = product.Name,
            Price = product.Price
        };

        patchDocument.ApplyTo(patchModel);
        
        product.Name = patchModel.Name;
        product.Price = patchModel.Price ?? product.Price ;

        if(!repository.UpdateProduct(product))
        {
            return StatusCode(500,"Internal Server error");
        }
        return NoContent();
    }
    [HttpDelete("{productId:guid}")]
    public IActionResult DeleteProduct(Guid productId)
    {
        if (repository.ExistsById(productId))
        {
            return NotFound("Product not found");
        }

        var succeeded = repository.DeleteProduct(productId);

        if (!succeeded)
        {
            return StatusCode(500,"Internal server error");
        }
        return NoContent();
    }

    // accepted response
    [HttpPost("process")]
    public IActionResult Process()
    {
        var id = Guid.NewGuid();

        return Accepted($"api/products/status/{id}",new {id,status = "Processing"});
    }
    [HttpGet("status/{id:guid}")]
    public IActionResult Process(Guid id)
    {
        bool requested = false;
        return Ok(new {id,status = requested ? "Processing": "Completed"});
    }
}




