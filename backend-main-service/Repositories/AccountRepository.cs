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

    public async Task<string?> GetId(string userName) {
        return await _context.Users.Where(u => u.NormalizedUserName == userName.ToUpper()).Select(u => u.Id)
            .FirstOrDefaultAsync();
    }

    public async Task<(string?, string?)> GetIds(string firstUserName, string secondUserName) {
        var senderId = await GetId(firstUserName);
        var recipientId = await GetId(secondUserName);

        return (senderId, recipientId);
    }

    public async Task<AppUser?> GetByName(string userName) {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.NormalizedUserName == userName.ToUpper());
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

    public async Task<AppUser[]> GetAccounts<OrderT>(QueryFilter<AppUser, OrderT> filter) {
        IQueryable<AppUser> users = _context.Users;

        // Filtering
        if (filter.Predicates != null)
            foreach (var predicate in filter.Predicates)
                users = users.Where(predicate);

        // Ordering
        users = filter.IsAscending ?
            users.OrderBy(filter.KeyOrder) :
            users.OrderByDescending(filter.KeyOrder);

        // Pagination
        users = users.Skip(filter.Skip).Take(filter.Take);

        return await users.Select(u => new AppUser {
            Id = u.Id,
            UserName = u.UserName
        }).ToArrayAsync();
    }

    public async Task<bool> ContainsByNameCaseIndep(string userName) {
        var upperName = userName.ToUpper();
        return await _context.Users.AnyAsync(u => u.NormalizedUserName == upperName);
    }

    public async Task<bool> ContainsByNameCaseDep(string userName) {
        return await _context.Users.AnyAsync(u => u.UserName == userName);
    }

    public async Task<bool> ContainsByEmail(string email) {
        var upperEmail = email.ToUpper();
        return await _context.Users.AnyAsync(u => u.NormalizedEmail == upperEmail);
    }

    public async Task<bool> ContainsById(string id) {
        return await _context.Users.AnyAsync(u => u.Id == id);
    }
}
