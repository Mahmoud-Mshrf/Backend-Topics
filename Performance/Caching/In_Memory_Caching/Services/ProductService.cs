using In_Memory_Caching.Controllers;
using In_Memory_Caching.Data;
using In_Memory_Caching.Models;
using In_Memory_Caching.Requests;
using In_Memory_Caching.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace In_Memory_Caching.Services;

public class ProductService(AppDbContext context,IMemoryCache cache) : IProductService
{
    private const string cacheKey = "Products";

    public async Task<List<ProductResponse>> GetProductsAsync()
    {
        return (await cache.GetOrCreateAsync(cacheKey,async x =>
        {
            x.Size = 1;
            x.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
            x.SlidingExpiration = TimeSpan.FromMinutes(5);
/*
* AbsoluteExpirationRelativeToNow:** Sets a fixed lifetime for a cached item starting from the moment it is added to the cache. Once that time is reached, 
  the item is removed even if it has been accessed many times.
  For example, if you cache a list of products with an absolute expiration of **30 minutes**, it will expire exactly 30 minutes after being cached, whether it was requested once or a hundred times.

* SlidingExpiration:** Sets an inactivity timeout for a cached item instead of a fixed lifetime.
  Every time the item is accessed, the expiration timer is reset, so it remains in the cache as long as it continues to be used.
  For example, if you set a sliding expiration of **30 minutes** and users access the cached products every 10 minutes, the cache will never expire; however, if no one requests the products for 30 minutes, the item will be removed from the cache.

*/
            var entities = await context.Products.ToListAsync();
            var responses =  entities?.Select(p => ProductResponse.FromModel(p)).ToList() ?? [];
            return responses;
        }))!;   
    }
    public async Task<List<ProductResponse>> GetProductsAsync_OldWay()
    {
        if (cache.TryGetValue(cacheKey,out List<ProductResponse>? products))
        {
            return products!;
        }
        var entities = await context.Products.ToListAsync();
        products =  entities?.Select(p => ProductResponse.FromModel(p)).ToList() ?? [];
        cache.Set(cacheKey,products,new MemoryCacheEntryOptions
        {
            Size=1,
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
        });
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
        cache.Remove(cacheKey); // invalidate the cache
        return ProductResponse.FromModel(product);
    }

    public async Task UpdateProductAsync(int productId, UpdateProductRequest request)
    {
        var existingProduct = await context.Products.FirstOrDefaultAsync(p => p.Id == productId)
                                ?? throw new KeyNotFoundException("product not found");

        existingProduct.Name = request.Name;

        existingProduct.Price = request.Price;
        cache.Remove(cacheKey); // invalidate the cache
        await context.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == id)
                      ?? throw new KeyNotFoundException("product not found");

        context.Products.Remove(product);
        cache.Remove(cacheKey); // invalidate the cache
        await context.SaveChangesAsync();
    }
}
