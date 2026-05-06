using System.ComponentModel.DataAnnotations;

namespace DocShareApi.Dtos.Accounts;

public record UsersFastQuery(
    [Required, Range(1, 25)]
    int Take,
    string? UserName = ""
);