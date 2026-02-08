using System.Text;
using Microsoft.AspNetCore.Identity;
namespace Auth.CustomException;

public class BusinessLogicException : Exception {

    private static readonly string defaultMessage
        = "No message provided.";

    public BusinessLogicException(
        string? message
    ) : base(message ?? defaultMessage) {

    }

    public BusinessLogicException(
        string? message,
        Exception? innerException
    ) : base(message ?? defaultMessage, innerException) {
        
    }

    public static void ThrowIfLessThan(
        int actualValue,
        int compareValue,
        string nameOfParameter
    ) {
        if (actualValue < compareValue) {
            throw new BusinessLogicException(
                $"{nameOfParameter} cannot be less than {compareValue}. " + 
                $"Actual value: {actualValue}.");
        }
    }

    public static void ThrowIfGreaterThan(
        int actualValue,
        int compareValue,
        string nameOfParameter
    ) {
        if (actualValue > compareValue) {
            throw new BusinessLogicException(
                $"{nameOfParameter} cannot be greater than {compareValue}. " + 
                $"Actual value: {actualValue}.");
        }
    }

    public static void ThrowIfLessThanOrEqual(
        int actualValue,
        int compareValue,
        string nameOfParameter
    ) {
        if (actualValue <= compareValue) {
            throw new BusinessLogicException(
                $"{nameOfParameter} cannot be less than or equal to {compareValue}. " + 
                $"Actual value: {actualValue}.");
        }
    }

    public static void ThrowIfGreaterThanOrEqual(
        int actualValue,
        int compareValue,
        string nameOfParameter
    ) {
        if (actualValue >= compareValue) {
            throw new BusinessLogicException(
                $"{nameOfParameter} cannot be greater than or equal to {compareValue}. " + 
                $"Actual value: {actualValue}.");
        }
    }

    public static void ThrowIfInRange(
        int actualValue,
        int minValue,
        int maxValue,
        string nameOfParameter
    ) {
        if (actualValue >= minValue && actualValue <= maxValue) {
            throw new BusinessLogicException(
                $"{nameOfParameter} cannot be in range [{minValue}, {maxValue}]. " + 
                $"Actual value: {actualValue}.");
        }
    }

    public static void ThrowIfNotInRange(
        int actualValue,
        int minValue,
        int maxValue,
        string nameOfParameter
    ) {
        if (actualValue < minValue || actualValue > maxValue) {
            throw new BusinessLogicException(
                $"{nameOfParameter} must be in range [{minValue}, {maxValue}]. " + 
                $"Actual value: {actualValue}.");
        }
    }

    public static void ThrowIfEqual(
        int actualValue,
        int compareValue,
        string nameOfParameter
    ) {
        if (actualValue == compareValue) {
            throw new BusinessLogicException(
                $"{nameOfParameter} cannot be equal to {compareValue}. " + 
                $"Actual value: {actualValue}.");
        }
    }

    public static void ThrowIfNotEqual(
        int actualValue,
        int compareValue,
        string nameOfParameter
    ) {
        if (actualValue != compareValue) {
            throw new BusinessLogicException(
                $"{nameOfParameter} must be equal to {compareValue}. " + 
                $"Actual value: {actualValue}.");
        }
    }
}