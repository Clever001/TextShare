using System;
using System.ComponentModel.DataAnnotations;

namespace TextShareApi.Attributes;

public class NonEmptyStringAttribute : ValidationAttribute {
    protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
        string? str = value as string;

        if (str != null && str.Trim() == "") {
            return new ValidationResult($"{validationContext.DisplayName} cannot be empty.");
        }

        return ValidationResult.Success!;
    }
}