using System.ComponentModel.DataAnnotations;

namespace DocShareApi.Dtos.Accounts;

public sealed class UpdateUserDto {
    [Required, Length(5, 25)] 
    public string? UserName { get; init; } = null;

    [Required, EmailAddress] 
    public string? Email { get; init; } = null;
}
