using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Shared;
using TextShareApi.Dtos.Accounts;
using TextShareApi.Dtos.QueryOptions;
using Shared.ApiError;
using TextShareApi.Interfaces.Repositories;
using TextShareApi.Interfaces.Services;
using TextShareApi.Mappers;
using TextShareApi.Models;

namespace TextShareApi.Services;

public class AccountService : IAccountService {
    private readonly IAccountRepository _accountRepository;
    private readonly ITokenService _tokenService;
    private readonly UserManager<AppUser> _userManager;

    public AccountService(ITokenService tokenService, UserManager<AppUser> userManager,
        IAccountRepository accountRepository) {
        _tokenService = tokenService;
        _userManager = userManager;
        _accountRepository = accountRepository;
    }

    public async Task<Result<(AppUser, string)>> Register(string userName, string email, string password) {
        try {
            var user = new AppUser {
                UserName = userName,
                Email = email
            };
            var createResult = await _userManager.CreateAsync(user, password);
            if (createResult.Succeeded) {
                var appendResult = await _userManager.AddToRoleAsync(user, "User");
                if (appendResult.Succeeded) {
                    var token = _tokenService.CreateToken(user);
                    return Result<(AppUser, string)>.Success((user, token));
                }

                await _userManager.DeleteAsync(user);

                return Result<(AppUser, string)>.Failure(new ServerApiError());
            }

            return Result<(AppUser, string)>.Failure(new BadRequestApiError(
                "Cannot create user with provided information.",
                createResult.Errors.Select(e => e.Description).ToList()));
        }
        catch (Exception e) {
            return Result<(AppUser, string)>.Failure(new ServerApiError());
        }
    }

    public async Task<Result<(AppUser, string)>> Login(string nameOrEmail, string password) {
        var user = await _userManager.FindByNameAsync(nameOrEmail);
        if (user == null) user = await _userManager.FindByEmailAsync(nameOrEmail);

        if (user == null) return Result<(AppUser, string)>.Failure(new UnauthorizedApiError());

        var validPassword = await _userManager.CheckPasswordAsync(user, password);
        if (!validPassword) return Result<(AppUser, string)>.Failure(new UnauthorizedApiError());

        var token = _tokenService.CreateToken(user);
        return Result<(AppUser, string)>.Success((user, token));
    }

    public async Task<Result<(AppUser, string)>> Update(string userName, UpdateUserDto update) {
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null) return Result<(AppUser, string)>.Failure(new ServerApiError());

        if (update.UserName != null && update.UserName != user.UserName) {
            var exists = await _accountRepository.ContainsAccountByName(update.UserName);
            if (exists)
                return Result<(AppUser, string)>.Failure(
                    new BadRequestApiError("Account with such userName already exists."));

            user.UserName = update.UserName;
        }

        if (update.Email != null && update.Email != user.Email) {
            var exists = await _accountRepository.ContainsAccountByEmail(update.Email);
            if (exists)
                return Result<(AppUser, string)>.Failure(
                    new BadRequestApiError("Account with such email already exists."));

            user.Email = update.Email;
        }

        await _userManager.UpdateAsync(user);
        var token = _tokenService.CreateToken(user);

        return Result<(AppUser, string)>.Success((user, token));
    }

    public async Task<Result<PaginatedResponseDto<AppUser>>> GetUsers(PaginationDto pagination, string? userName) {
        var skip = (pagination.PageNumber - 1) * pagination.PageSize;
        var take = pagination.PageSize;
        Expression<Func<AppUser, string?>> orderBy = u => u.UserName;
        Expression<Func<AppUser, bool>>? nameFilter = null;
        if (userName != null && userName.Trim() != "")
            nameFilter = u => u.UserName!.ToLower().Contains(userName.ToLower());

        var (count, users) = await _accountRepository.GetAllAccounts(
            skip,
            take,
            orderBy,
            true,
            nameFilter == null ? null : [nameFilter]
        );

        return Result<PaginatedResponseDto<AppUser>>.Success(users.ToPaginatedResponse(pagination, count));
    }
}