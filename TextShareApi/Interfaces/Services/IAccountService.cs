using TextShareApi.ClassesLib;
using TextShareApi.Dtos.Accounts;
using TextShareApi.Dtos.QueryOptions;
using TextShareApi.Models;

namespace TextShareApi.Interfaces.Services;

public interface IAccountService
{
    Task<Result<(AppUser, string)>> Register(string userName, string email, string password);
    Task<Result<(AppUser, string)>> Login(string nameOrEmail, string password);
    Task<Result<(AppUser, string)>> Update(string userName, UpdateUserDto update);
    Task<Result<PaginatedResponseDto<AppUser>>> GetUsers(PaginationDto pagination, string? userName);
}