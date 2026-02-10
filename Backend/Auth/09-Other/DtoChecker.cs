using System.Text.RegularExpressions;

namespace Auth.Other;

public partial class DtoChecker {
    private readonly List<string> errors = new();
    [GeneratedRegex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*\.[a-zA-Z]{2,}$")]
    private static partial Regex EmailRegex();

    public void AppendOtherDtoCheckResult(DtoCheckResult result) {
        errors.AddRange(result.Errors);
    }

    public void AddErrorIfNotEmail(string parameter, string parameterName) {
        if (string.IsNullOrWhiteSpace(parameter) || !IsValidEmail(parameter)) {
            errors.Add(
                $"{parameterName} is not a valid email address."
            );
        }
    }

    private static bool IsValidEmail(string email) {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        return EmailRegex().IsMatch(email);
    }

    public void AddErrorIfNullOrEmptyString(string parameter, string parameterName) {
        if (string.IsNullOrWhiteSpace(parameter)) {
            errors.Add(
                $"{parameterName} is required."
            );
        }
    }

    public void AddErrorIfNotNullEmptyString(string? parameter, string parameterName) {
        if (parameter != null && parameter == string.Empty) {
            errors.Add(
                $"{parameterName} can be null but cannot be empty."
            );
        }
    }

    public void AddErrorIfValueIsLessThan(
        int actualValue,
        int compareValue,
        string nameOfParameter
    ) {
        if (actualValue < compareValue) {
            errors.Add(
                $"{nameOfParameter} cannot be less than {compareValue}. " +
                $"Actual value: {actualValue}."
            );
        }
    }

    public void AddErrorIfValueIsGreaterThan(
        int actualValue,
        int compareValue,
        string nameOfParameter
    ) {
        if (actualValue > compareValue) {
            errors.Add(
                $"{nameOfParameter} cannot be greater than {compareValue}. " +
                $"Actual value: {actualValue}."
            );
        }
    }

    public void AddErrorIfValueIsLessThanOrEqual(
        int actualValue,
        int compareValue,
        string nameOfParameter
    ) {
        if (actualValue <= compareValue) {
            errors.Add(
                $"{nameOfParameter} cannot be less than or equal to {compareValue}. " +
                $"Actual value: {actualValue}."
            );
        }
    }

    public void AddErrorIfValueIsGreaterThanOrEqual(
        int actualValue,
        int compareValue,
        string nameOfParameter
    ) {
        if (actualValue >= compareValue) {
            errors.Add(
                $"{nameOfParameter} cannot be greater than or equal to {compareValue}. " +
                $"Actual value: {actualValue}."
            );
        }
    }

    public void AddErrorIfValueIsInRange(
        int actualValue,
        int minValue,
        int maxValue,
        string nameOfParameter
    ) {
        if (actualValue >= minValue && actualValue <= maxValue) {
            errors.Add(
                $"{nameOfParameter} cannot be in range [{minValue}, {maxValue}]. " +
                $"Actual value: {actualValue}."
            );
        }
    }

    public void AddErrorIfValueIsNotInRange(
        int actualValue,
        int minValue,
        int maxValue,
        string nameOfParameter
    ) {
        if (actualValue < minValue || actualValue > maxValue) {
            errors.Add(
                $"{nameOfParameter} must be in range [{minValue}, {maxValue}]. " +
                $"Actual value: {actualValue}."
            );
        }
    }

    public void AddErrorIfValueIsEqual(
        int actualValue,
        int compareValue,
        string nameOfParameter
    ) {
        if (actualValue == compareValue) {
            errors.Add(
                $"{nameOfParameter} cannot be equal to {compareValue}. " +
                $"Actual value: {actualValue}."
            );
        }
    }

    public void AddErrorIfValueIsNotEqual(
        int actualValue,
        int compareValue,
        string nameOfParameter
    ) {
        if (actualValue != compareValue) {
            errors.Add(
                $"{nameOfParameter} must be equal to {compareValue}. " +
                $"Actual value: {actualValue}."
            );
        }
    }

    public DtoCheckResult GetCheckResult() {
        return new DtoCheckResult(
            IsValid: errors.Count == 0,
            Errors: errors.ToArray()
        );
    }

    public record DtoCheckResult(
        bool IsValid,
        string[] Errors
    );
}