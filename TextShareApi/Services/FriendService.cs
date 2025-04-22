using Microsoft.AspNetCore.Identity;
using TextShareApi.Interfaces;
using TextShareApi.Models;

namespace TextShareApi.Services;

public class FriendService : IFriendService {
    private readonly IFriendPairRepository _fpRepo;
    private readonly UserManager<AppUser> _userManager;

    public FriendService(IFriendPairRepository fpRepo, UserManager<AppUser> userManager) {
        _fpRepo = fpRepo;
        _userManager = userManager;
    }
    
    public Task<Result<bool>> AddFriend(string firstUserName, string secondUserName) {
        throw new NotImplementedException();
    }

    public Task<Result<List<AppUser>>> GetFriends(string userName) {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> RemoveFriend(string firstUserName, string secondUserName) {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> AreFriends(string firstUserName, string secondUserName) {
        throw new NotImplementedException();
    }
}