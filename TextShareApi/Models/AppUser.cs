using Microsoft.AspNetCore.Identity;

namespace TextShareApi.Models;

public class AppUser :IdentityUser {
    public List<Text> Texts { get; set; } = new List<Text>();
}