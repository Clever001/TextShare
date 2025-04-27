using System.ComponentModel.DataAnnotations;

namespace TextShareApi.Dtos.Accounts;

public class RegisterDto {
    [Required] [Length(5, 25)] public string UserName { get; set; } = string.Empty;

    [Required] [EmailAddress] public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}