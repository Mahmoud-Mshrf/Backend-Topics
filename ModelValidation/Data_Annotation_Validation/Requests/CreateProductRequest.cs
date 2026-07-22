namespace Data_Annotation_Validation.Requests;

public class CreateProductRequest
{
    public string? Name {get;set;}
    public string? Description {get;set;}
    public string? SKU {get;set;}
    public decimal Price {get;set;}
    public int StockQuantity {get;set;}
    public DateTime LaunchDate {get;set;}
    public ProductCategory Category {get;set;}
    public string? ImageUrl {get;set;}
    public decimal Weight {get;set;}
    public int WarrantyPeriodMonths {get;set;}
    public bool IsReturnable {get;set;}
    public string? ReturnPolicyDescription {get;set;}
    public List<string> Tags {get;set;} = [];
}
