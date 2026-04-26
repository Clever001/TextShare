using System.ComponentModel.DataAnnotations;

namespace DocShareApi.Dtos.Accounts;

public sealed class UpdateUserDto {
    [Length(5, 25)] public string? UserName { get; init; } = null;

    [EmailAddress] public string? Email { get; init; } = null;
}
