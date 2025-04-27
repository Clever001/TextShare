using Microsoft.AspNetCore.Identity;
using TextShareApi.ClassesLib;
using TextShareApi.Interfaces.Services;
using TextShareApi.Models;

namespace TextShareApi.Services;

public class AccountService : IAccountService {
    // TODO: Разобраться с регистрацией входа и выхода из системы
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly UserManager<AppUser> _userManager;

    public AccountService(ITokenService tokenService, UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager) {
        _tokenService = tokenService;
        _userManager = userManager;
        _signInManager = signInManager;
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

                return Result<(AppUser, string)>.Failure(appendResult.ToString(), false);
            }

            return Result<(AppUser, string)>.Failure(createResult.ToString(), false);
        }
        catch (Exception e) {
#if DEBUG
            return Result<(AppUser, string)>.Failure(e.ToString(), false);
#else
                return Result<(AppUser, string)>.Failure(e.Message, false);
#endif
        }
    }

    public async Task<Result<(AppUser, string)>> Login(string nameOrEmail, string password) {
        var user = await _userManager.FindByNameAsync(nameOrEmail);
        if (user == null) user = await _userManager.FindByEmailAsync(nameOrEmail);

        if (user == null) return Result<(AppUser, string)>.Failure("Invalid username or password.", true);

        var validPassword = await _userManager.CheckPasswordAsync(user, password);
        if (!validPassword) return Result<(AppUser, string)>.Failure("Invalid username or password.", true);

        var token = _tokenService.CreateToken(user);
        return Result<(AppUser, string)>.Success((user, token));
    }

    /*
    public async Task<Result> Logout(string nameOrEmail) {
        throw new NotImplementedException();
    }*/
}