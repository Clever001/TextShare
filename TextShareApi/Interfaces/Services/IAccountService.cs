using TextShareApi.ClassesLib;
using TextShareApi.Models;

namespace TextShareApi.Interfaces.Services;

public interface IAccountService {
    Task<Result<(AppUser, string)>> Register(string userName, string email, string password);
    Task<Result<(AppUser, string)>> Login(string nameOrEmail, string password);
    Task<Result> Logout(string nameOrEmail);
}