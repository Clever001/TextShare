using Auth.Model;
using Auth.Other;
using Microsoft.AspNetCore.Identity;

namespace Auth.Repository.Interface;

public interface IUserRepository {
    Task<IdentityResult> CreateUser(User user, string password);
    Task<IdentityResult> AddUserToRole(User user, string role);
    Task<User?> FindById(string userId);
    Task<User?> FindByName(string userName);
    Task<User?> FindByEmail(string userEmail);
    Task<bool> ContainsById(string userId);
    Task<bool> ContainsByName(string userName);
    Task<bool> ContainsByEmail(string userEmail);
    Task<bool> IsValidPassword(User user, string password);
    Task<(int countOfUsers, List<User> users)> GetUsersCollection<KeyOrderT>(
        QueryFilter<User, KeyOrderT> queryFilter
    );
    Task<IdentityResult> UpdateUser(User user);
    Task<IdentityResult> DeleteUser(User user);
}