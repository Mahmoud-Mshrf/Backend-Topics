using Distributed_Caching.Requests;
using Distributed_Caching.Responses;

namespace Distributed_Caching.Services;

public interface IProductService
{
    Task<List<ProductResponse>> GetProductsAsync();

    Task<ProductResponse?> GetProductByIdAsync(int productId);

    Task<ProductResponse> AddProductAsync(CreateProductRequest request);

    Task UpdateProductAsync(int productId, UpdateProductRequest request);

    Task DeleteProductAsync(int id);
}
