using System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Class)]
public class OneOfRequiredAttribute : ValidationAttribute {
    private readonly string _firstProperty;
    private readonly string _secondProperty;

    public OneOfRequiredAttribute(string firstProperty, string secondProperty) {
        _firstProperty = firstProperty;
        _secondProperty = secondProperty;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
        var firstValue = validationContext.ObjectType.GetProperty(_firstProperty)
            ?.GetValue(validationContext.ObjectInstance);
        var secondValue = validationContext.ObjectType.GetProperty(_secondProperty)
            ?.GetValue(validationContext.ObjectInstance);

        if (firstValue == null && secondValue == null)
            return new ValidationResult($"One of '{_firstProperty}' or '{_secondProperty}' must be provided.");

        return ValidationResult.Success;
    }
}