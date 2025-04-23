using Microsoft.AspNetCore.Identity;
using TextShareApi.Interfaces.Services;
using TextShareApi.Models;

namespace TextShareApi.Services;

public class AccountRepository : IAccountRepository {
    private readonly UserManager<AppUser> _userManager;

    public AccountRepository(UserManager<AppUser> userManager) {
        _userManager = userManager;
    }
    
    public async Task<string?> GetAccountId(string userName) {
        return (await _userManager.FindByNameAsync(userName))?.Id;
    }
}