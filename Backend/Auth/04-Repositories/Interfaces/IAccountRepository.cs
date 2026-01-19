using System.Linq.Expressions;
using Auth.Models;

namespace Auth.Repositories.Interfaces;

public interface IAccountRepository {
    Task<string?> GetAccountId(string userName);
    Task<(string?, string?)> GetAccountIds(string firstUserName, string secondUserName);
    Task<AppUser?> GetAccountByName(string userName);

    Task<(int, List<AppUser>)> GetAllAccounts<T>(int skip,
        int take,
        Expression<Func<AppUser, T>> keyOrder,
        bool isAscending,
        List<Expression<Func<AppUser, bool>>>? predicates);

    Task<bool> ContainsAccountByName(string userName);
    Task<bool> ContainsAccountByEmail(string email);
}