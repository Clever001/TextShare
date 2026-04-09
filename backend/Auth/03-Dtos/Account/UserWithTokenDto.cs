using Auth.Model;
using Auth.Other;

namespace Auth.Dto.Account;

public record UserWithTokenDto(
    User User,
    string Token
);