namespace TextShareApi.Models.Enums;

public enum SecurityCheckResult {
    Allowed,
    Forbidden,
    NoPasswordProvided,
    PasswordIsNotValid,
    NotFound,
}