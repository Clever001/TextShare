using TextShareApi.ClassesLib;
using TextShareApi.Dtos.QueryOptions;
using TextShareApi.Models;

namespace TextShareApi.Interfaces.Services;

public interface IAccountService {
    Task<Result<(AppUser, string)>> Register(string userName, string email, string password);
    Task<Result<(AppUser, string)>> Login(string nameOrEmail, string password);
    Task<Result<List<AppUser>>> GetUsers(PaginationDto pagination, string? userName);
}