using TextShareApi.Models;

namespace TextShareApi.Interfaces.Repositories;

public interface IAccountRepository {
    Task<string?> GetAccountId(string userName);
    Task<string?> GetUserName(string userId);
    Task<IList<AppUser>> GetUsers();
    Task<AppUser?> GetAccountById(string userId);
    Task<AppUser?> GetAccountByName(string userName);
    Task<AppUser?> GetTextOwner(string textId);
}