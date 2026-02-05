namespace Auth.Dto.Account;

public record UpdateUserDto(
    string InitialUserId,
    string? NewName,
    string? NewEmail
);
