using System.ComponentModel.DataAnnotations;

namespace DocShareApi.Dtos.Accounts;

public record FullUserDto(
    [Required]
    string Id,
    [Required]
    string UserName,
    [Required]
    string Email,
    [Required]
    DateTime CreatedOn
);