using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using TextShareApi.ClassesLib;
using TextShareApi.Dtos.QueryOptions;
using TextShareApi.Exceptions;
using TextShareApi.Interfaces.Repositories;
using TextShareApi.Interfaces.Services;
using TextShareApi.Mappers;
using TextShareApi.Models;

namespace TextShareApi.Services;

public class AccountService : IAccountService {
    private readonly ITokenService _tokenService;
    private readonly UserManager<AppUser> _userManager;
    private readonly IAccountRepository _accountRepository;

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

                return Result<(AppUser, string)>.Failure(new ServerException());
            }

            return Result<(AppUser, string)>.Failure(new BadRequestException(
                "Cannot create user with provided information.",
                createResult.Errors.Select(e => e.Description).ToList()));
        }
        catch (Exception e) {
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

    public async Task<Result<PaginatedResponseDto<AppUser>>> GetUsers(PaginationDto pagination, string? userName) {
        int skip = (pagination.PageNumber - 1) * pagination.PageSize;
        int take = pagination.PageSize;
        Expression<Func<AppUser, string?>> orderBy = u => u.UserName;
        Expression<Func<AppUser, bool>>? nameFilter = null;
        if (userName != null && userName.Trim() != "") nameFilter = u => u.UserName!.ToLower().Contains(userName.ToLower());

        var (count, users) = await _accountRepository.GetAllAccounts(
            skip: skip,
            take: take,
            keyOrder: orderBy,
            isAscending: true,
            predicates: nameFilter == null ?
                null:
                [nameFilter]
        );

        return Result<PaginatedResponseDto<AppUser>>.Success(users.ToPaginatedResponse(pagination, count));
    }
}