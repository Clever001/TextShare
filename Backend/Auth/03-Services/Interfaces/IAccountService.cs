using Auth.Models;
using Auth.Other;
using Shared;

namespace Auth.Services.Interfaces;

public interface IAccountService {
    Task<Result<(AppUser, string)>> Register(string userName, string email, string password);
    Task<Result<(AppUser, string)>> Login(string nameOrEmail, string password);
    Task<Result<(AppUser, string)>> Update(string userName, UpdateUserCommand update);
    Task<Result<PaginatedResponse<AppUser>>> GetUsers(PaginationCommand pagination, string? userName);
}