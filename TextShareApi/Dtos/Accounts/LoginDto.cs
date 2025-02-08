using System.ComponentModel.DataAnnotations;

namespace TextShareApi.Dtos.Accounts;

public class LoginDto { 
    [Required]
    public string UserNameOrEmail { get; set; }
    [Required]
    public string Password { get; set; }
}