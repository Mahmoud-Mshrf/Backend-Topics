using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Data_Annotation_Validation.Validators;

public static class LaunchDateValidator
{
    public static ValidationResult? MustBeTodayOrFuture(DateTime dateTime,ValidationContext context)
    {
        if (dateTime.Date >= DateTime.UtcNow.Date)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult("Launch date must be today or in the future");
    }
    
}
