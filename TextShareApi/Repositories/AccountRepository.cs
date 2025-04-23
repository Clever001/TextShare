using Microsoft.AspNetCore.Identity;
using TextShareApi.Interfaces.Repositories;
using TextShareApi.Models;

namespace TextShareApi.Repositories;

public class AccountRepository : IAccountRepository {
    private readonly UserManager<AppUser> _userManager;

    public AccountRepository(UserManager<AppUser> userManager) {
        _userManager = userManager;
    }
    
    public async Task<string?> GetAccountId(string userName) {
        return (await _userManager.FindByNameAsync(userName))?.Id;
    }

    public async Task<string?> GetUserName(string userId) {
        return (await _userManager.FindByIdAsync(userId))?.UserName;
    }

    public async Task<IList<AppUser>> GetUsers() {
        return await _userManager.GetUsersInRoleAsync("User");
    }
}