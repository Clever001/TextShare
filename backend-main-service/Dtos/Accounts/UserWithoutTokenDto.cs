using System.ComponentModel.DataAnnotations;

namespace DocShareApi.Dtos.Accounts;

public sealed class UserWithoutTokenDto {
    [Required]
    public string Id { get; init; } = string.Empty;
    [Required]
    public string UserName { get; init; } = string.Empty;
}
