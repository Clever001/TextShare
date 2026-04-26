using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DocShareApi.Data;
using DocShareApi.Models;
using DocShareApi.Dtos.QueryOptions.Filters;

namespace DocShareApi.Repositories;

public class AccountRepository : IAccountRepository {
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public AccountRepository(UserManager<AppUser> userManager,
        AppDbContext context) {
        _userManager = userManager;
        _context = context;
    }

    public async Task<string?> GetAccountId(string userName) {
        return await _context.Users.Where(u => u.NormalizedUserName == userName.ToUpper()).Select(u => u.Id)
            .FirstOrDefaultAsync();
    }

    public async Task<(string?, string?)> GetAccountIds(string firstUserName, string secondUserName) {
        var senderId = await GetAccountId(firstUserName);
        var recipientId = await GetAccountId(secondUserName);

        return (senderId, recipientId);
    }

    public async Task<AppUser?> GetAccountByName(string userName) {
        return await _userManager.FindByNameAsync(userName);
    }

    public async Task<FilterResult<AppUser>> GetAllAccounts<OrderT>(QueryFilter<AppUser, OrderT> filter) {
        IQueryable<AppUser> users = _context.Users;

        // Filtering
        if (filter.Predicates != null)
            foreach (var predicate in filter.Predicates)
                users = users.Where(predicate);

        var count = await users.CountAsync();

        // Ordering
        users = filter.IsAscending ? 
            users.OrderBy(filter.KeyOrder) : 
            users.OrderByDescending(filter.KeyOrder);

        // Pagination
        users = users.Skip(filter.Skip).Take(filter.Take);

        return new FilterResult<AppUser>(count, await users.Select(u => new AppUser {
            Id = u.Id,
            UserName = u.UserName
        }).ToListAsync());
    }

    public async Task<bool> ContainsAccountByName(string userName) {
        var upperName = userName.ToUpper();
        return await _context.Users.AnyAsync(u => u.NormalizedUserName == upperName);
    }

    public async Task<bool> ContainsAccountByEmail(string email) {
        var upperEmail = email.ToUpper();
        return await _context.Users.AnyAsync(u => u.NormalizedEmail == upperEmail);
    }

    public async Task<bool> ContainsAccountById(string id) {
        return await _context.Users.AnyAsync(u => u.Id == id);
    }
}
