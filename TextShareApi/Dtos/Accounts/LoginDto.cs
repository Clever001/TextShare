using System.ComponentModel.DataAnnotations;

namespace TextShareApi.Dtos.Accounts;

[OneOfRequired(nameof(UserName), nameof(Email))]
public class LoginDto { 
    public string? UserName { get; set; }
    public string? Email { get; set; }
    [Required]
    public string Password { get; set; }
}