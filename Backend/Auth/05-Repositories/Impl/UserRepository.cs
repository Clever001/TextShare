using Auth.CustomException;
using Auth.DbContext;
using Auth.Model;
using Auth.Other;
using Auth.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

namespace Auth.Repository.Impl;

public class UserRepository (
    AppDbContext dbContext,
    UserManager<User> userManager
) : IUserRepository {
    private readonly DbSet<User> dbSet = dbContext.Users;

    public async Task<EntityResult> CreateUser(User user, string password) {
        var creationResult = await userManager.CreateAsync(user);
        if (!creationResult.Succeeded) {
            return creationResult.ToEntityResult();
        }
        
        var addingToRoleResult = await userManager.AddToRoleAsync(user, "User");
        if (!addingToRoleResult.Succeeded) {
            await userManager.DeleteAsync(user);
            throw DbCallException.CreateFromIdentityErrors(
                "Couldn't add user to role", 
                addingToRoleResult.Errors
            );
        }

        return creationResult.ToEntityResult();
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
        return await dbSet.AnyAsync(
            u => u.Id == userId
        );
    }

    public async Task<bool> ContainsByName(string userName) {
        return await dbSet.AnyAsync(
            u => u.NormalizedUserName == userName.ToUpperInvariant()
        );
    }

    public async Task<bool> ContainsByEmail(string userEmail) {
        return await dbSet.AnyAsync(
            u => u.NormalizedEmail == userEmail.ToUpperInvariant()
        );
    }

    public async Task<bool> IsValidPassword(User user, string password) {
        return await userManager.CheckPasswordAsync(user, password);
    }

    public async Task<SelectionOfItems<User>> 
    GetUsersCollection<KeyOrderT>(
        QueryFilter<User, KeyOrderT> queryFilter
    ) {
        var (countOfUsers, users) =
            await queryFilter.ApplyFilter(dbSet);

        return new SelectionOfItems<User>(
            TotalCount: countOfUsers,
            Selection: await users.Select(u => new User {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email
            }).ToArrayAsync()
        );
    }

    public async Task<EntityResult> UpdateUser(User user) {
        return (
            await userManager.UpdateAsync(user)
        ).ToEntityResult();
    }

    public async Task<EntityResult> DeleteUser(User user) {
        // TODO: implement delete from UserDocumentRoleAssignment
        return (
            await userManager.DeleteAsync(user)
        ).ToEntityResult();
    }
}