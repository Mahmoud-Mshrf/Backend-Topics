using Distributed_Caching.Data;
using Distributed_Caching.Models;
using Distributed_Caching.Requests;
using Distributed_Caching.Responses;
using Microsoft.EntityFrameworkCore;

namespace Distributed_Caching.Services;

public class ProductService(AppDbContext context) : IProductService
{
    public async Task<List<ProductResponse>> GetProductsAsync()
    {
        var products = await context.Products.ToListAsync();
        return products?.Select(p => ProductResponse.FromModel(p)).ToList() ?? [];
        
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

        return ProductResponse.FromModel(product);
    }

    public async Task UpdateProductAsync(int productId, UpdateProductRequest request)
    {
        var existingProduct = await context.Products.FirstOrDefaultAsync(p => p.Id == productId)
                                ?? throw new KeyNotFoundException("product not found");

        existingProduct.Name = request.Name;

        existingProduct.Price = request.Price;

        await context.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == id)
                      ?? throw new KeyNotFoundException("product not found");

        context.Products.Remove(product);

        await context.SaveChangesAsync();
    }
}
