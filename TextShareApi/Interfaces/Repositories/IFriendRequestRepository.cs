using System.Linq.Expressions;
using TextShareApi.Models;

namespace TextShareApi.Interfaces.Repositories;

public interface IFriendRequestRepository {
    Task<FriendRequest> CreateRequest(string senderId, string recipientId);
    Task<FriendRequest?> GetRequest(string senderId, string recipientId);
    Task<List<FriendRequest>> GetFriendRequests(Expression<Func<FriendRequest, bool>> predicate);
    Task<FriendRequest?> UpdateRequest(string senderId, string recipientId, bool isAccepted);
    Task<bool> DeleteRequest(string senderId, string recipientId);
    Task<bool> ContainsRequest(string senderId, string recipientId);
}