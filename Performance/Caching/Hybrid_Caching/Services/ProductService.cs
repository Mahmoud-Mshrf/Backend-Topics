using In_Memory_Caching.Data;
using In_Memory_Caching.Models;
using In_Memory_Caching.Requests;
using In_Memory_Caching.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Caching.Hybrid;

namespace In_Memory_Caching.Services;

public class ProductService(AppDbContext context,HybridCache cache) : IProductService
{
    private const string cacheKey = "Products";
    public async Task<List<ProductResponse>> GetProductsAsync()
    {
        var products = await cache.GetOrCreateAsync(cacheKey,
        async ct =>
        {
            var entities = await context.Products.ToListAsync(ct);

            var productResponse = entities?.Select(p => ProductResponse.FromModel(p)).ToList() ?? [];

            Console.WriteLine("DB Visited");

            return productResponse;
        },
        options: new HybridCacheEntryOptions
        {

        },
        tags: ["products-tag"] // we can use it to remove by tag in case of two different entities but with related data and removing one should effect the other : await cache.RemoveByTagAsync(tagName);
        );

        return products;
    }

    public async Task<ProductResponse?> GetProductByIdAsync(int productId)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == productId);
        return product is null ? null : ProductResponse.FromModel(product);
    }

    public async Task<ProductResponse> AddProductAsync(CreateProductRequest request)
    {
        var product = new Product
        {
            Name = request.Name,
            Price = request.Price
        };

        context.Products.Add(product);

        await context.SaveChangesAsync();
        await cache.RemoveAsync(cacheKey);
        return ProductResponse.FromModel(product);
    }

    public async Task UpdateProductAsync(int productId, UpdateProductRequest request)
    {
        var existingProduct = await context.Products.FirstOrDefaultAsync(p => p.Id == productId)
                                ?? throw new KeyNotFoundException("product not found");

        existingProduct.Name = request.Name;

        existingProduct.Price = request.Price;

        await context.SaveChangesAsync();
        await cache.RemoveAsync(cacheKey);

    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == id)
                      ?? throw new KeyNotFoundException("product not found");

        context.Products.Remove(product);

        await context.SaveChangesAsync();
        await cache.RemoveAsync(cacheKey);

    }
}
