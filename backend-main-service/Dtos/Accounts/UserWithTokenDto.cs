using System.ComponentModel.DataAnnotations;

namespace DocShareApi.Dtos.Accounts;

public sealed class UserWithTokenDto {
    [Required]
    public string Id { get; set; } = string.Empty;
    [Required]
    public string UserName { get; init; } = string.Empty;
    [Required]
    public string Email { get; init; } = string.Empty;
    [Required]
    public string Token { get; init; } = string.Empty;
}
