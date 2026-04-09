using System.ComponentModel.DataAnnotations;

namespace TextShareApi.Attributes;

public class ValidTagsAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var tags = value as List<string>;
        if (tags == null) return ValidationResult.Success!;

        tags = tags.Distinct().ToList();

        if (tags.Count > 5)
        {
            return new ValidationResult("Tags list cannot contain more then 5 elements.");
        }

        foreach (string tag in tags)
        {
            if (tag.Length < 1 || tag.Length > 20)
            {
                return new ValidationResult("Each tag length should be between 1 and 20");
            }
        }

        return ValidationResult.Success!;
    }
}