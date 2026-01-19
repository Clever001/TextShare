using Auth.Models;
using Shared;

namespace Auth.Services.Interfaces;

public interface IFriendService {
    Task<Result> AddFriend(string firstUserName, string secondUserName);

    Task<Result<PaginatedResponseDto<AppUser>>> GetFriends(PaginationDto pagination, bool isAscending,
        string? friendName, string senderName);

    Task<Result> RemoveFriend(string firstUserName, string secondUserName);
    Task<Result<bool>> AreFriendsByName(string firstUserName, string secondUserName);
    Task<Result<bool>> AreFriendsById(string firstUserId, string secondUserId);
}