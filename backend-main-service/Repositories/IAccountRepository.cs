using System.Linq.Expressions;
using DocShareApi.Dtos.QueryOptions.Filters;
using DocShareApi.Models;

namespace DocShareApi.Repositories;

public interface IAccountRepository {
    Task<string?> GetAccountId(string userName);
    Task<(string?, string?)> GetAccountIds(string firstUserName, string secondUserName);
    Task<AppUser?> GetAccountByName(string userName);

    Task<FilterResult<AppUser>> GetAllAccounts<OrderT>(
        QueryFilter<AppUser, OrderT> filter
    );

    Task<bool> ContainsAccountByName(string userName);
    Task<bool> ContainsAccountByEmail(string email);
    Task<bool> ContainsAccountById(string id);
}
