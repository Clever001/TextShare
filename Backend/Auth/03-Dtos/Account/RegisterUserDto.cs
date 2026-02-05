namespace Auth.Dto.Account;

public record RegisterUserDto(
    string Name,
    string Email,
    string Password
);
