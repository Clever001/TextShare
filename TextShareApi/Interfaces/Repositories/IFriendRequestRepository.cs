using System.Linq.Expressions;
using TextShareApi.Models;

namespace TextShareApi.Interfaces.Repositories;

public interface IFriendRequestRepository {
    Task<FriendRequest> CreateRequest(string senderId, string recipientId);
    Task<FriendRequest?> GetRequest(string senderId, string recipientId);
    Task<(int, List<FriendRequest>)> GetFriendRequests<T>(
        int skip, int take, Expression<Func<FriendRequest, T>> keyOrder,
        bool isAscending,
        List<Expression<Func<FriendRequest, bool>>>? predicates);
    Task<FriendRequest?> UpdateRequest(string senderId, string recipientId, bool isAccepted);
    Task<bool> DeleteRequest(string senderId, string recipientId);
    Task<bool> ContainsRequest(string senderId, string recipientId);
}