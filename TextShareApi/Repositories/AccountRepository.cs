using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TextShareApi.Data;
using TextShareApi.Interfaces.Repositories;
using TextShareApi.Models;

namespace TextShareApi.Repositories;

public class AccountRepository : IAccountRepository {
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public AccountRepository(UserManager<AppUser> userManager,
        AppDbContext context) {
        _userManager = userManager;
        _context = context;
    }

    public async Task<string?> GetAccountId(string userName) {
        return await _context.Users.Where(u => u.NormalizedUserName == userName.ToUpper()).Select(u => u.Id).FirstOrDefaultAsync();
    }

    public async Task<(string?, string?)> GetAccountIds(string firstUserName, string secondUserName) {
        var senderId = await GetAccountId(firstUserName);
        var recipientId = await GetAccountId(secondUserName);

        return (senderId, recipientId);
    }

    public async Task<AppUser?> GetAccountByName(string userName) {
        return await _userManager.FindByNameAsync(userName);
    }

    public async Task<(int, List<AppUser>)> GetAllAccounts<T>(int skip,
        int take,
        Expression<Func<AppUser, T>> keyOrder,
        bool isAscending,
        List<Expression<Func<AppUser, bool>>>? predicates) 
    {
        IQueryable<AppUser> users = _context.Users;
        
        // Filtering
        if (predicates != null) {
            foreach (var predicate in predicates) {
                users = users.Where(predicate);
            }
        }
        
        int count = await users.CountAsync();
        
        // Ordering
        users = isAscending ? 
            users.OrderBy(keyOrder) : 
            users.OrderByDescending(keyOrder);
        
        // Pagination
        users = users.Skip(skip).Take(take);
        return (count, await users.Select(u => new AppUser {
            Id = u.Id,
            UserName = u.UserName,
        }).ToListAsync());
    }

    public async Task<bool> ContainsAccountByName(string userName) {
        string upperName = userName.ToUpper();
        return await _context.Users.AnyAsync(u => u.NormalizedUserName == upperName);
    }

    public async Task<bool> ContainsAccountByEmail(string email) {
        string upperEmail = email.ToUpper();
        return await _context.Users.AnyAsync(u => u.NormalizedEmail == upperEmail);
    }
}