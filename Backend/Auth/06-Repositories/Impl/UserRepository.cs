using Auth.DbContext;
using Auth.Model;
using Auth.Other;
using Auth.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Auth.Repository.Impl;

public class UserRepository (
    AppDbContext dbContext,
    UserManager<User> userManager
) : IUserRepository {
    public async Task<IdentityResult> CreateUser(User user, string password) {
        return await userManager.CreateAsync(user, password);
    }

    public async Task<IdentityResult> AddUserToRole(User user, string role) {
        return await userManager.AddToRoleAsync(user, "User");
    }

    public async Task<User?> FindById(string userId) {
        return await userManager.FindByIdAsync(userId);
    }

    public async Task<User?> FindByName(string userName) {
        return await userManager.FindByNameAsync(userName);
    }

    public async Task<User?> FindByEmail(string userEmail) {
        return await userManager.FindByEmailAsync(userEmail);
    }

    public async Task<bool> ContainsById(string userId) {
        return await dbContext.Users.AnyAsync(u => u.Id == userId);
    }

    public async Task<bool> ContainsByName(string userName) {
        return await dbContext.Users.AnyAsync(u => 
            u.NormalizedUserName == userName.ToUpperInvariant()
        );
    }

    public async Task<bool> ContainsByEmail(string userEmail) {
        return await dbContext.Users.AnyAsync(u => 
            u.Email == userEmail.ToUpperInvariant()
        );
    }

    public async Task<bool> IsValidPassword(User user, string password) {
        return await userManager.CheckPasswordAsync(user, password);
    }

    public async Task<(int countOfUsers, List<User> users)> 
    GetUsersCollection<KeyOrderT>(
        QueryFilter<User, KeyOrderT> queryFilter
    ) {
        IQueryable<User> users = dbContext.Users;

        if (queryFilter.ContainsWherePredicates) {
            foreach (var predicate in queryFilter.WherePredicates) {
                users = users.Where(predicate);
            }
        }

        var countOfUsers = await users.CountAsync();

        if (queryFilter.ContainsSortFilter) {
            var sortingKey = queryFilter.SortingKey;
            var shouldSortAscending = queryFilter.ShouldSortAscending;
            if (shouldSortAscending) {
                users = users.OrderBy(sortingKey);
            } else {
                users = users.OrderByDescending(sortingKey);
            }
        }

        if (queryFilter.ContainsPaginationFilter) {
            int skip = queryFilter.SkipItemsCount;
            int take = queryFilter.TakeItemsCount;
            users = users.Skip(skip).Take(take);
        }

        return (countOfUsers, await users.Select(u => new User {
            Id = u.Id,
            UserName = u.UserName,
            Email = u.Email
        }).ToListAsync());
    }

    public async Task<IdentityResult> UpdateUser(User user) {
        return await userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult> DeleteUser(User user) {
        // TODO: implement delete from UserDocumentRoleAssignment
        throw new NotImplementedException();
    }
}