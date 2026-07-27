using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Output_Caching.Data;
using Output_Caching.Models;
using Output_Caching.Requests;
using Output_Caching.Responses;

namespace Output_Caching.Services;

public class ProductService(AppDbContext context) : IProductService
{

    public async Task<PagedResult<ProductResponse>> GetProductsAsync(int page = 1, int pageSize = 10)
    {
        page = Math.Max(1, page);

        pageSize = Math.Clamp(pageSize, 1, 10);

        var entities = await context.Products
            .OrderBy(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var totalCount = await context.Products.CountAsync();

        var products = entities?.Select(p => ProductResponse.FromModel(p)).ToList() ?? [];

        return new PagedResult<ProductResponse>
        {
            CurrentPage = page,
            PageSize = pageSize,
            Items = products,
            TotalCount = totalCount
        };
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
