using System.ComponentModel.DataAnnotations;

namespace TextShareApi.Attributes;

public class ExpiryDateAttribute : ValidationAttribute {
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
        switch (value) {
            case null:
                return ValidationResult.Success;
            case DateTime dt: {
                var maxAllowed = DateTime.UtcNow.AddMinutes(10);

                if (dt > maxAllowed) return ValidationResult.Success;

                return new ValidationResult("The expiry date cannot be later than the current date + 10 minutes.");
            }
            default:
                return new ValidationResult("Invalid expiry date format.");
        }
    }
}