using System.Net.Http.Headers;
using WebAppWithEntityFramework.Models;

namespace WebAppWithEntityFramework.Dtos;
public class ProductResponse
{
    public Guid Id {get;set;}
    public string Name {get;set;}
    public decimal Price {get;set;}
    public List<ReviewResponse>? reviews {get;set;} = default;

    private  ProductResponse()
    {
        
    }
    public static ProductResponse FromModel(Product product,IEnumerable<ProductReview>? reviews = null)
    {
        if (product == null)
        {
            throw new ArgumentNullException(nameof(product));
        }

        var response = new ProductResponse
        {
            Id=product.Id,
            Name=product.Name,
            Price=product.Price,
        };

        if (reviews != null)
        {
            response.reviews = ReviewResponse.FromModels(reviews).ToList();
        }
        return response;
    }
    public static IEnumerable<ProductResponse> FromModels(IEnumerable<Product> products)
    {
        if (products == null)
        {
            throw new ArgumentNullException(nameof(products),"products collection can't be null");
        }
        return products.Select(p=> FromModel(p));
    }
}
