using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.VisualBasic;
using Minimal_WebApi.Data;
using Minimal_WebApi.Models;

namespace Minimal_WebApi.Endpoints;

public static class ProductsEndpoints
{
    public static RouteGroupBuilder MapProductsEnpoints (this IEndpointRouteBuilder routeBuilder)
    {
        var productsGroup = routeBuilder.MapGroup("/api/products");
        productsGroup.MapGet("/text", () => "Hello World!");

        productsGroup.MapGet("/json", () =>
        {
            return new { Name = "Product from json"}; 
        });

        productsGroup.MapGet("/i-result/{id:guid}", (System.Guid id , ProductRepository repository) =>
        {
        var product = repository.GetProductById(id);

        return product == null   
                ? Results.NotFound()
                : Results.Ok(product);
        });

        productsGroup.MapGet("/type-result/{id:guid}", Results<Ok<Product>,NotFound> (System.Guid id , ProductRepository repository) =>
        {
        var product = repository.GetProductById(id);

        return product == null   
                ? TypedResults.NotFound()
                : TypedResults.Ok(product);
        });

        return productsGroup;
    }
}