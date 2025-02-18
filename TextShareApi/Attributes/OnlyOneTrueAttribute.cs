using System.ComponentModel.DataAnnotations;

namespace TextShareApi.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class OnlyOneTrueAttribute : ValidationAttribute {
    private string[] _properties;

    public OnlyOneTrueAttribute(params string[] properties) {
        _properties = properties;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
        int counter = 0;
        foreach (var propertyName in _properties) {
            var curBooleanValue = validationContext.ObjectType.GetProperty(propertyName)
                ?.GetValue(validationContext.ObjectInstance) as bool?;
            if (curBooleanValue == true) counter++;
        }

        if (counter != 1) {
            return new ValidationResult("Only one true property can be set.", _properties);
        }
        
        return ValidationResult.Success;
    }
}