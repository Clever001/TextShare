using TextShareApi.Models;

namespace TextShareApi.Interfaces;

public interface IFriendPairsRepository {
    Task<List<string>> GetFriendsIds(string userId);
}