using System.Linq.Expressions;
using Auth.Models;

namespace Auth.Interfaces.Repositories;

public interface IFriendPairRepository {
    Task<List<string>> GetFriendsIds(string userId);

    Task<ValueTuple<FriendPair, FriendPair>>
        CreateFriendPairs(string firstUserId, string secondUserId);

    Task<FriendPair?> GetFriendPair(string firstUserId, string secondUserId);

    Task<(int, List<FriendPair>)> GetFriendPairs<T>(int skip,
        int take,
        Expression<Func<FriendPair, T>> keyOrder,
        bool isAscending,
        List<Expression<Func<FriendPair, bool>>>? predicates,
        bool includeSender,
        bool includeRecipient);

    Task<bool> DeleteFriendPairs(string firstUserId, string secondUserId);
    Task<bool> ContainsFriendPair(string firstUserId, string secondUserId);
}