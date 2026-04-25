using Microsoft.AspNetCore.Identity;

namespace TextShareApi.Models;

public class AppUser : IdentityUser {
    public List<Text> Texts { get; set; } = new();
    public List<FriendRequest> FriendRequests { get; set; } = new();
    public List<FriendPair> FriendPairs { get; set; } = new();
}