using Microsoft.AspNetCore.Identity;

namespace TextShareApi.Models;

public class AppUser :IdentityUser {
    public List<Text> Texts { get; set; } = new List<Text>();
    public List<FriendRequest> FriendRequests { get; set; } = new List<FriendRequest>();
    public List<FriendPair> FriendPairs { get; set; } = new List<FriendPair>();
}