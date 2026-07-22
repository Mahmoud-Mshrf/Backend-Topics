using FluentValidation;
using FluentValidation_MinimalApi.Requests;

namespace FluentValidation_MinimalApi.Validators;

public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        // validation is here
        RuleFor(x=>x.Name)
        .NotEmpty().WithMessage("Product name is required")
        .Length(3,255).WithMessage("Product name must be between 3 and 255 characters");

        RuleFor(x=>x.Description)
        .MaximumLength(1000).WithMessage("Product description can't exceed 1000 characters");

        RuleFor(x=>x.SKU)
        .NotEmpty().WithMessage("Product SKU is required")
        .Matches(@"^PRD-\d{5}$").WithMessage("SKU must be 'PRD-' followed by 5 digits 'PRD-XXXXX'");

        RuleFor(x=>x.Price)
        .GreaterThan(0).WithMessage("Product price must be at least 0.01");

        RuleFor(x=>x.StockQuantity)
        .GreaterThanOrEqualTo(0).WithMessage("Stock Quantity for a product must be a non-negative integer");

        RuleFor(x=>x.LaunchDate)
        .Must(x=>x.Date >= DateTime.UtcNow.Date)
        .WithMessage("Launch Date for a product must be today or in the future");

        RuleFor(x=>x.ImageUrl)
        .Must(x =>
        {
             return Uri.TryCreate(x,UriKind.Absolute,out _);
        }).WithMessage("ImageUrl is not a valid url");
        
        RuleFor(x=>x.Weight)
        .InclusiveBetween(0.01m,1000m).WithMessage("Product weight must be between 0.01 and 1000 kg");

        RuleFor(x=>x.WarrantyPeriodMonths)
        .Must(x =>
        {
           return x == 0 || x == 12 || x == 18 || x == 24 || x == 30 || x == 36; 
        }).WithMessage("Product warranty must be between 1 and 36 months");

        When(x=> x.IsReturnable,() =>
        {
            RuleFor(x=>x.ReturnPolicyDescription)
            .NotEmpty().WithMessage("Return policy description is required if the product is returnable");
        }) ;

        RuleFor(x=>x.Tags)
        .Must(x=>x.Count<=5)
        .WithMessage("A maximum of 5 tags is allowed");
    }
}