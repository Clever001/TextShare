using DocShareApi.Dtos.QueryOptions.Filters;
using DocShareApi.Models;

namespace DocShareApi.Repositories;

public interface IAccountRepository {
    Task<string?> GetId(string userName);
    Task<(string?, string?)> GetIds(string firstUserName, string secondUserName);
    Task<AppUser?> GetByName(string userName);

    Task<FilterResult<AppUser>> GetAllAccounts<OrderT>(
        QueryFilter<AppUser, OrderT> filter
    );
    Task<AppUser[]> GetAccounts<OrderT> (
        QueryFilter<AppUser, OrderT> filter
    );

    Task<bool> ContainsByNameCaseIndep(string userName);
    Task<bool> ContainsByNameCaseDep(string userName);
    Task<bool> ContainsByEmail(string email);
    Task<bool> ContainsById(string id);
}
