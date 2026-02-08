using Auth.Model;
using Auth.Other;
using Shared.Result;

namespace Auth.Repository.Interface;

public interface IUserRepository {
    Task<EntityResult> CreateUser(User user, string password);
    Task<User?> FindById(string userId);
    Task<User?> FindByName(string userName);
    Task<User?> FindByEmail(string userEmail);
    Task<bool> IsValidPassword(User user, string password);
    Task<SelectionOfItems<User>> GetUsersCollection<KeyOrderT>(
        QueryFilter<User, KeyOrderT> queryFilter
    );
    Task<EntityResult> UpdateUser(User user);
    Task<EntityResult> DeleteUser(User user);
}