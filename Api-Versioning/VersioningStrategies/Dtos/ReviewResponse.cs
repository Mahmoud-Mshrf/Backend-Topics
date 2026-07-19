using VersioningStrategies.Models;

namespace VersioningStrategies.Dtos;

public class ReviewResponse
{
    public Guid Id {get;set;}
    public Guid ProductId {get;set;}
    public string Reviewer {get;set;}
    public int Stars {get;set;}

    private ReviewResponse()
    {
        
    }

    public static ReviewResponse FromModel(ProductReview review)
    {
        if (review ==null)
        {
            throw new ArgumentNullException(nameof(review));
        }

        return new ReviewResponse
        {
            Id=review.Id,
            ProductId=review.ProductId,
            Reviewer=review.Reviewer,
            Stars=review.Stars
        };
    }

    public static IEnumerable<ReviewResponse> FromModels(IEnumerable<ProductReview> reviews)
    {
        if (reviews == null)
        {
            throw new ArgumentNullException(nameof(reviews));            
        }

        return reviews.Select(FromModel);
    }
}