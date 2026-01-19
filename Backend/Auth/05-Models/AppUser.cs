using Microsoft.AspNetCore.Identity;

namespace Auth.Models;

public class AppUser : IdentityUser {
    public List<FriendRequest> FriendRequests {get; set;} = new();
    public List<FriendPair> FriendPairs {get; set;} = new();
}