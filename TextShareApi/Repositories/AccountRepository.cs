using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TextShareApi.Data;
using TextShareApi.Interfaces.Repositories;
using TextShareApi.Models;

namespace TextShareApi.Repositories;

public class AccountRepository : IAccountRepository {
    private readonly UserManager<AppUser> _userManager;
    private readonly AppDbContext _context;

    public AccountRepository(UserManager<AppUser> userManager,
        AppDbContext context) {
        _userManager = userManager;
        _context = context;
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

    public async Task<AppUser?> GetAccount(string userId) {
        return await _userManager.FindByIdAsync(userId);
    }

    public async Task<AppUser?> GetTextOwner(string textId) {
        return (await _context.Texts
            .Include(text => text.AppUser)
            .FirstOrDefaultAsync(text => text.Id == textId))?.AppUser;
    }
}