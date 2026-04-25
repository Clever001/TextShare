namespace DocShareApi.Dtos.Enums;

public enum SecurityCheckResult {
    Allowed,
    Forbidden,
    NoPasswordProvided,
    PasswordIsNotValid,
    NotFound
}