using System.ComponentModel.DataAnnotations;

namespace SampleApi.Validation;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class CustomValidationAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if(value != null && value.ToString() == "Yury")
            return true;

        return false;
    }
}
