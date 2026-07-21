namespace WebAppWithEntityFramework.Models;

public class Product
{
    public Guid Id {get;set;}
    public string Name {get;set;}
    public decimal Price {get;set;}
    public ICollection<ProductReview> ProductReviews { get; set; }
        = new List<ProductReview>();
}
