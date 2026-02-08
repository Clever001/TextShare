using Auth.CustomException;

namespace Auth.Validator;

public abstract class Validator {
    protected bool? isValid = null;
    protected ICollection<string> errors = new List<string>();

    public abstract void PerformValidityCheck();
    public bool IsValid {
        get {
            if (isValid == null) {
                throw new BusinessLogicException(
                    "Tried to see validation result before preforming check."
                );
            }

            return isValid.Value;
        }
    }
    public IEnumerable<string> ValidationErrors {
        get => errors;
    }

    protected void SetInvalidIfValueIsLessThan(
        int actualValue,
        int compareValue,
        string nameOfParameter
    ) {
        if (actualValue < compareValue) {
            isValid = false;
            errors.Add(
                $"{nameOfParameter} cannot be less than {compareValue}. " +
                $"Actual value: {actualValue}."
            );
        }
    }

    protected void SetInvalidIfValueIsGreaterThan(
        int actualValue,
        int compareValue,
        string nameOfParameter
    ) {
        if (actualValue > compareValue) {
            isValid = false;
            errors.Add(
                $"{nameOfParameter} cannot be greater than {compareValue}. " + 
                $"Actual value: {actualValue}."
            );
        }
    }

    protected void SetInvalidIfValueIsLessThanOrEqual(
        int actualValue,
        int compareValue,
        string nameOfParameter
    ) {
        if (actualValue <= compareValue) {
            isValid = false;
            errors.Add(
                $"{nameOfParameter} cannot be less than or equal to {compareValue}. " + 
                $"Actual value: {actualValue}."
            );
        }
    }

    protected void SetInvalidIfValueIsGreaterThanOrEqual(
        int actualValue,
        int compareValue,
        string nameOfParameter
    ) {
        if (actualValue >= compareValue) {
            isValid = false;
            errors.Add(
                $"{nameOfParameter} cannot be greater than or equal to {compareValue}. " + 
                $"Actual value: {actualValue}."
            );
        }
    }

    protected void SetInvalidIfValueIsInRange(
        int actualValue,
        int minValue,
        int maxValue,
        string nameOfParameter
    ) {
        if (actualValue >= minValue && actualValue <= maxValue) {
            isValid = false;
            errors.Add(
                $"{nameOfParameter} cannot be in range [{minValue}, {maxValue}]. " + 
                $"Actual value: {actualValue}."
            );
        }
    }

    protected void SetInvalidIfValueIsNotInRange(
        int actualValue,
        int minValue,
        int maxValue,
        string nameOfParameter
    ) {
        if (actualValue < minValue || actualValue > maxValue) {
            isValid = false;
            errors.Add(
                $"{nameOfParameter} must be in range [{minValue}, {maxValue}]. " + 
                $"Actual value: {actualValue}."
            );
        }
    }

    protected void SetInvalidIfValueIsEqual(
        int actualValue,
        int compareValue,
        string nameOfParameter
    ) {
        if (actualValue == compareValue) {
            isValid = false;
            errors.Add(
                $"{nameOfParameter} cannot be equal to {compareValue}. " + 
                $"Actual value: {actualValue}."
            );
        }
    }

    protected void SetInvalidIfValueIsNotEqual(
        int actualValue,
        int compareValue,
        string nameOfParameter
    ) {
        if (actualValue != compareValue) {
            isValid = false;
            errors.Add(
                $"{nameOfParameter} must be equal to {compareValue}. " + 
                $"Actual value: {actualValue}."
            );
        }
    }
}