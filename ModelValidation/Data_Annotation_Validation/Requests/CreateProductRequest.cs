using System.ComponentModel.DataAnnotations;
using Data_Annotation_Validation.Validators;

namespace Data_Annotation_Validation.Requests;

public class CreateProductRequest
{
    [Required(ErrorMessage ="Product name is required")]
    [StringLength(255,MinimumLength =3,ErrorMessage ="Product name must be between 3 and 255 characters")]
    public string? Name {get;set;}
    [StringLength(1000,ErrorMessage ="Product description can't exceed 1000 characters")]// optional
    public string? Description {get;set;}
    [Required(ErrorMessage ="Product sku is required")]
    [RegularExpression(@"^PRD-\d{5}$",ErrorMessage ="SKU must be 'PRD-' followed by 5 digits 'PRD-XXXXX'")]
    public string? SKU {get;set;}
    [Range(0.01,double.MaxValue,ErrorMessage ="Product price must be at least 0.01")]
    public decimal Price {get;set;}
    [Range(0,int.MaxValue,ErrorMessage ="Integer stock quantity value is required")]
    public int StockQuantity {get;set;}
    [Required(ErrorMessage ="Product launch date is required")]
    [CustomValidation(typeof(LaunchDateValidator),nameof(LaunchDateValidator.MustBeTodayOrFuture))]
    public DateTime LaunchDate {get;set;}
    [EnumDataType(typeof(ProductCategory),ErrorMessage = "Invalid product category")]
    public ProductCategory Category {get;set;}
    [Url(ErrorMessage ="ImageUrl must be a valid url")]
    public string? ImageUrl {get;set;}
    [Range(0.01,1000,ErrorMessage ="Product weight must be between 0.01 and 1000 kg")]
    public decimal Weight {get;set;}
    [Range(0,36,ErrorMessage ="Product warranty must be between 1 and 36 months")]
    [CustomValidation(typeof(WarrantyPeriodValidator),nameof(WarrantyPeriodValidator.MustBe_0_12_18_24_30_36))]
    public int WarrantyPeriodMonths {get;set;}
    public bool IsReturnable {get;set;}
    [RequiredIf("IsReturnable",true,ErrorMessage ="Return policy description is required if the product is returnable")]
    public string? ReturnPolicyDescription {get;set;}
    [MaxLength(5,ErrorMessage ="A maximum of 5 tags is allowed")]
    public List<string> Tags {get;set;} = [];
}

