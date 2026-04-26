using Microsoft.AspNetCore.Identity;

namespace DocShareApi.Models;

public class AppUser : IdentityUser {
    public List<Document> Documents { get; set; } = new();
    public List<UserToDocRole> Roles { get; set; } = new();
}
