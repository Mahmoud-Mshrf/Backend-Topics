using System.ComponentModel.DataAnnotations;

namespace Data_Annotation_InMinimalApi.Validators;

public class RequiredIfAttribute : ValidationAttribute
{
    private readonly string _dependentProperty;
    private readonly object? _targetValue;

    public RequiredIfAttribute(string dependentProperty, object? targetValue)
    {
        _dependentProperty = dependentProperty;
        _targetValue = targetValue;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var containerType = validationContext.ObjectInstance.GetType();// the type that the properties inside it
        var field = containerType.GetProperty(_dependentProperty);// the dependent property

        if (field == null )
        {
            return new ValidationResult($"Unknown property : {_dependentProperty}");
        }

        var dependentValue = field.GetValue(validationContext.ObjectInstance,null);// the value of the dependent property

        if (Equals(dependentValue,_targetValue))// here we check if the dependentValue is equal to the targetValue that we need it to match
        {
            // here we validate the depending property the property that depends on the dependant property
            // this mean that because the dependent property is matched with the target value then the depending property is required
            if (value == null || value is string str && string.IsNullOrWhiteSpace(str))
            {
                return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} is required",new [] {validationContext.MemberName?? "ReturnPolicyDescription"});
            }
        }

        return  ValidationResult.Success;
    } 
}