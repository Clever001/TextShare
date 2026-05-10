using System.ComponentModel.DataAnnotations;

namespace DocShareApi.Dtos.Accounts;

public record UserWithoutTokenDto(
    [Required]
    string Id,
    [Required]
    string UserName
);
