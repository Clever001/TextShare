namespace Auth.Dto.Account;

public record LoginUserDto(
    string NameOrEmail,
    string Password
);
