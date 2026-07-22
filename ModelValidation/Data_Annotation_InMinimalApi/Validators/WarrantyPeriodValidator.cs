using System.ComponentModel.DataAnnotations;

namespace Data_Annotation_InMinimalApi.Validators;

public static class WarrantyPeriodValidator
{
    public static ValidationResult? MustBe_0_12_18_24_30_36(int warranty,ValidationContext context)
    {
        if (warranty == 0 || warranty == 12 || warranty == 18 || warranty == 24 || warranty == 30 || warranty == 36)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult("Warranty Period Months must be 0 , 12 , 18 , 24 , 30 or 36 months",new [] {context.MemberName??"WarrantyPeriodMonths"});
    }
}