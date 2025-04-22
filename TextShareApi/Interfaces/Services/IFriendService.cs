using TextShareApi.Models;

namespace TextShareApi.Interfaces;

public interface IFriendService {
    Task<Result<bool>> AddFriend(string firstUserName, string secondUserName);
    Task<Result<List<AppUser>>> GetFriends(string userName);
    Task<Result<bool>> RemoveFriend(string firstUserName, string secondUserName);
    Task<Result<bool>> AreFriends(string firstUserName, string secondUserName);
}