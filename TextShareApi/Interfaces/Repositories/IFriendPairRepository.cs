using System.Linq.Expressions;
using TextShareApi.Models;

namespace TextShareApi.Interfaces.Repositories;

public interface IFriendPairRepository {
    Task<List<string>> GetFriendsIds(string userId);
    Task<ValueTuple<FriendPair, FriendPair>> CreateFriendPairs(string firstUserId, string secondUserId); // TODO: Вынести логику создания в сервис.
    Task<FriendPair?> GetFriendPair(string firstUserId, string secondUserId);
    Task<List<FriendPair>> GetFriendPairs(Expression<Func<FriendPair, bool>> predicate);
    Task<bool> DeleteFriendPairs(string firstUserId, string secondUserId);
    Task<bool> ContainsFriendPair(string firstUserId, string secondUserId);
}