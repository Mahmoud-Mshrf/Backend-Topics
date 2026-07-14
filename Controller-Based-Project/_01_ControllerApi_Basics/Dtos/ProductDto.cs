using System.Net.Http.Headers;
using _01_ControllerApi_Basics.Models;

namespace _01_ControllerApi_Basics.Dtos;
public class ProductDto
{
    public Guid Id {get;set;}
    public string Name {get;set;}
    public decimal Price {get;set;}
    public List<ReviewDto>? reviews {get;set;} = default;

    private  ProductDto()
    {
        
    }
    public static ProductDto FromModel(Product product,IEnumerable<ProductReview>? reviews = null)
    {
        if (product == null)
        {
            throw new ArgumentNullException(nameof(product));
        }

        var response = new ProductDto
        {
            Id=product.Id,
            Name=product.Name,
            Price=product.Price,
        };

        if (reviews != null)
        {
            response.reviews = ReviewDto.FromModels(reviews).ToList();
        }
        return response;
    }
    public static IEnumerable<ProductDto> FromModels(IEnumerable<Product> products)
    {
        if (products == null)
        {
            throw new ArgumentNullException(nameof(products),"products collection can't be null");
        }
        return products.Select(p=> FromModel(p));
    }
}
