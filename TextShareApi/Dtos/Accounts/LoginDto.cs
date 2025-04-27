using System.ComponentModel.DataAnnotations;

namespace TextShareApi.Dtos.Accounts;

public class LoginDto { 
    [Required]
    public string UserNameOrEmail { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}