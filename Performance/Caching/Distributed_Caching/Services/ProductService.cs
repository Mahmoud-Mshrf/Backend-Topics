using System.Text.Json;
using Distributed_Caching.Data;
using Distributed_Caching.Models;
using Distributed_Caching.Requests;
using Distributed_Caching.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Distributed_Caching.Services;

public class ProductService(AppDbContext context,IDistributedCache cache) : IProductService
{
    private const string cacheKey = "Products";
    public async Task<List<ProductResponse>> GetProductsAsync()
    {
        var data = await cache.GetStringAsync(cacheKey);
        
        if (data is not null)
        {
            System.Console.WriteLine("Cache visited");
            return JsonSerializer.Deserialize<List<ProductResponse>>(data)!;  
        }

        var entities = await context.Products.ToListAsync();
        var products = entities?.Select(p => ProductResponse.FromModel(p)).ToList() ?? [];
        var jsonData = JsonSerializer.Serialize(products);
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow= TimeSpan.FromMinutes(30)
        };
        await cache.SetStringAsync(cacheKey,jsonData,options);
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
        await cache.RemoveAsync(cacheKey); // invalidate cache
        return ProductResponse.FromModel(product);
    }

    public async Task UpdateProductAsync(int productId, UpdateProductRequest request)
    {
        var existingProduct = await context.Products.FirstOrDefaultAsync(p => p.Id == productId)
                                ?? throw new KeyNotFoundException("product not found");

        existingProduct.Name = request.Name;

        existingProduct.Price = request.Price;

        await context.SaveChangesAsync();
        await cache.RemoveAsync(cacheKey); // invalidate cache
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == id)
                      ?? throw new KeyNotFoundException("product not found");

        context.Products.Remove(product);

        await context.SaveChangesAsync();
        await cache.RemoveAsync(cacheKey); // invalidate cache
    }
}
