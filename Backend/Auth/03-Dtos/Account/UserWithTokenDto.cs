using Auth.Model;

namespace Auth.Dto.Account;

public record UserWithTokenDto(
    User User,
    string Token
);
