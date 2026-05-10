using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using DocShareApi.ClassesLib;
using DocShareApi.Dtos.Accounts;
using DocShareApi.Dtos.QueryOptions;
using DocShareApi.Exceptions;
using DocShareApi.Repositories;
using DocShareApi.Mappers;
using DocShareApi.Models;
using DocShareApi.Dtos.QueryOptions.Filters;
using Microsoft.CodeAnalysis.CSharp;

namespace DocShareApi.Services;

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
                Email = email,
                CreatedOn = DateTime.UtcNow
            };
            var createResult = await _userManager.CreateAsync(user, password);
            if (createResult.Succeeded) {
                var appendResult = await _userManager.AddToRoleAsync(user, "User");
                if (appendResult.Succeeded) {
                    var token = _tokenService.CreateToken(user);
                    return Result<(AppUser, string)>.Success((user, token));
                }

                await _userManager.DeleteAsync(user);

                return Result<(AppUser, string)>.Failure(new ServerException());
            }

            return Result<(AppUser, string)>.Failure(new BadRequestException(
                "Cannot create user with provided information.",
                createResult.Errors.Select(e => e.Description).ToList()));
        } catch (Exception e) {
            return Result<(AppUser, string)>.Failure(new ServerException());
        }
    }

    public async Task<Result<(AppUser, string)>> Login(string nameOrEmail, string password) {
        var user = await _userManager.FindByNameAsync(nameOrEmail);
        if (user == null) user = await _userManager.FindByEmailAsync(nameOrEmail);

        if (user == null) return Result<(AppUser, string)>.Failure(new UnauthorizedException());

        var validPassword = await _userManager.CheckPasswordAsync(user, password);
        if (!validPassword) return Result<(AppUser, string)>.Failure(new UnauthorizedException());

        var token = _tokenService.CreateToken(user);
        return Result<(AppUser, string)>.Success((user, token));
    }

    public async Task<Result<AppUser>> GetByName(string userName) {
        var user = await _accountRepository.GetByName(userName);
        if (user == null) {
            return Result<AppUser>.Failure(new NotFoundException());
        }
        return Result<AppUser>.Success(user);
    }

    public async Task<Result<(AppUser, string)>> Update(string userName, UpdateUserDto update) {
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null) return Result<(AppUser, string)>.Failure(new ServerException());

        if (update.UserName != null && update.UserName != user.UserName) {
            var exists = await _accountRepository.ContainsByNameCaseDep(update.UserName);
            if (exists)
                return Result<(AppUser, string)>.Failure(
                    new BadRequestException("Account with such userName already exists."));

            user.UserName = update.UserName;
        }

        if (update.Email != null && update.Email != user.Email) {
            var exists = await _accountRepository.ContainsByEmail(update.Email);
            if (exists)
                return Result<(AppUser, string)>.Failure(
                    new BadRequestException("Account with such email already exists."));

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

        var filterResult = await _accountRepository.GetAllAccounts(
            new QueryFilter<AppUser, string?>(
                skip,
                take,
                orderBy,
                true,
                nameFilter == null ? null : [nameFilter]
            )
        );

        return Result<PaginatedResponseDto<AppUser>>.Success(
            filterResult.ToPaginatedResponse(
                pagination
            )
        );
    }

    public async Task<Result<AppUser[]>> GetUsersThatStartsWith(string userName, int take) {
        string uppercaseName = userName.ToUpperInvariant();

        Expression<Func<AppUser, bool>> nameStartsWithPredicate = u => u.NormalizedUserName!.StartsWith(uppercaseName);

        var users = await _accountRepository.GetAccounts(
            new QueryFilter<AppUser, string?>(
                Skip: 0,
                Take: take,
                KeyOrder: u => u.UserName,
                IsAscending: true,
                Predicates: [nameStartsWithPredicate]
            )
        );

        return Result<AppUser[]>.Success(users);
    }
}
