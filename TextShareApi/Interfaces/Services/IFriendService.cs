using TextShareApi.ClassesLib;
using TextShareApi.Models;

namespace TextShareApi.Interfaces.Services;

public interface IFriendService {
    Task<Result> AddFriend(string firstUserName, string secondUserName);
    Task<Result<List<AppUser>>> GetFriends(string userName);
    Task<Result> RemoveFriend(string firstUserName, string secondUserName);
    Task<Result<bool>> AreFriends(string firstUserName, string secondUserName);

    // Additional
    Task<Result<List<string>>> GetFriendsIds(string userId);
}