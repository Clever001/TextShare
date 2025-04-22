using System.Linq.Expressions;
using TextShareApi.Models;

namespace TextShareApi.Interfaces;

public interface IFriendPairRepository {
    // TODO: After implementing of FriendService include GetFriendsIds there.
    Task<List<string>> GetFriendsIds(string userId);
    Task<ValueTuple<FriendPair, FriendPair>> CreateFriendPairs(string firstUserId, string secondUserId);
    Task<FriendPair?> GetFriendPair(string firstUserId, string secondUserId);
    Task<List<FriendPair>> GetFriendPairs(Expression<Func<FriendPair, bool>> predicate);
    Task<bool> DeleteFriendPairs(string firstUserId, string secondUserId);
    Task<bool> ContainsFriendPair(string firstUserId, string secondUserId);
}