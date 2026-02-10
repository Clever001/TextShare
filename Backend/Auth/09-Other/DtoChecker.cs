namespace Auth.Other;

public class DtoChecker {
    private readonly List<string> errors = new();

    public void AppendOtherDtoCheckResult(DtoCheckResult result) {
        errors.AddRange(result.Errors);
    }

    public void CheckForRequiredString(string parameter, string parameterName) {
        if (string.IsNullOrWhiteSpace(parameter)) {
            errors.Add(
                $"{parameterName} is required."
            );
        }
    }

    public void SetInvalidIfValueIsLessThan(
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

    public void SetInvalidIfValueIsGreaterThan(
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

    public void SetInvalidIfValueIsLessThanOrEqual(
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

    public void SetInvalidIfValueIsGreaterThanOrEqual(
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

    public void SetInvalidIfValueIsInRange(
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

    public void SetInvalidIfValueIsNotInRange(
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

    public void SetInvalidIfValueIsEqual(
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

    public void SetInvalidIfValueIsNotEqual(
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

    public record DtoCheckResult (
        bool IsValid,
        string[] Errors
    );
}