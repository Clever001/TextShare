using TextShareApi.Models;
using TextShareApi.Repositories;

namespace TextShareApi.Interfaces;

public interface IFriendRequestRepository {
    Task<FriendRequest> CreateRequest(string userId, string recipientId);
    Task<FriendRequest?> GetRequest(string userId, string recipientId);
    Task<FriendRequest?> UpdateRequest(string userId, string recipientId, bool isAccepted);
    Task<bool> DeleteRequest(string userId, string recipientId);
    Task<bool> ContainsRequest(string userId, string recipientId);
}