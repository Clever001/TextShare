using DocShareApi.ClassesLib;
using DocShareApi.Dtos.Accounts;
using DocShareApi.Dtos.QueryOptions;
using DocShareApi.Models;

namespace DocShareApi.Services;

public interface IAccountService {
    Task<Result<(AppUser, string)>> Register(string userName, string email, string password);
    Task<Result<(AppUser, string)>> Login(string nameOrEmail, string password);
    Task<Result<(AppUser, string)>> Update(string userName, UpdateUserDto update);
    Task<Result<PaginatedResponseDto<AppUser>>> GetUsers(PaginationDto pagination, string? userName);
}
