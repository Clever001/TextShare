using TextShareApi.Models;

namespace TextShareApi.Interfaces;

public interface IFriendsRepository {
    Task<List<string>> GetFriendsIds(string userId);
}