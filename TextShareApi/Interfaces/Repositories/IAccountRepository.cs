using TextShareApi.Models;

namespace TextShareApi.Interfaces.Repositories;

public interface IAccountRepository {
    Task<string?> GetAccountId(string userName);
    Task<string?> GetUserName(string userId);
    Task<IList<AppUser>> GetUsers();
    Task<AppUser?> GetAccount(string userId);
    Task<AppUser?> GetTextOwner(string textId);
}