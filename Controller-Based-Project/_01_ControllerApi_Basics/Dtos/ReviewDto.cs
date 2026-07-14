using _01_ControllerApi_Basics.Models;

namespace _01_ControllerApi_Basics.Dtos;

public class ReviewDto
{
    public Guid Id {get;set;}
    public Guid ProductId {get;set;}
    public string Reviewer {get;set;}
    public int Stars {get;set;}

    private ReviewDto()
    {
        
    }

    public static ReviewDto FromModel(ProductReview review)
    {
        if (review ==null)
        {
            throw new ArgumentNullException(nameof(review));
        }

        return new ReviewDto
        {
            Id=review.Id,
            ProductId=review.ProductId,
            Reviewer=review.Reviewer,
            Stars=review.Stars
        };
    }

    public static IEnumerable<ReviewDto> FromModels(IEnumerable<ProductReview> reviews)
    {
        if (reviews == null)
        {
            throw new ArgumentNullException(nameof(reviews));            
        }

        return reviews.Select(FromModel);
    }
}