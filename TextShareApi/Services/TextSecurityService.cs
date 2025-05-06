using Microsoft.AspNetCore.Identity;
using TextShareApi.ClassesLib;
using TextShareApi.Exceptions;
using TextShareApi.Interfaces.Services;
using TextShareApi.Models;
using TextShareApi.Models.Enums;

namespace TextShareApi.Services;

public class TextSecurityService : ITextSecurityService {
    private readonly IFriendService _friendService;
    private readonly ILogger<TextSecurityService> _logger;
    private readonly PasswordHasher<AppUser> _passwordHasher;

    public TextSecurityService(PasswordHasher<AppUser> passwordHasher,
        IFriendService friendService,
        ILogger<TextSecurityService> logger) {
        _passwordHasher = passwordHasher;
        _friendService = friendService;
        _logger = logger;
    }

    public async Task<Result> PassReadSecurityChecks(Text text, AppUser? requestSender, string? password) {
        var textSecSettings = text.TextSecuritySettings;
        if (textSecSettings == null) {
            _logger.LogError("TextSecuritySettings is null");
            return Result.Failure(new ServerException());
        }

        switch (textSecSettings.AccessType) {
            case AccessType.ByReferencePublic: {
                return Result.Success();
            }
            case AccessType.ByReferenceAuthorized: {
                if (requestSender == null) return ForbiddenResult();

                break;
            }
            case AccessType.OnlyFriends: {
                if (requestSender == null) return ForbiddenResult();

                var areFriendsResult = await _friendService.AreFriends(text.AppUser.UserName!, requestSender.UserName!);

                if (text.AppUserId == requestSender.Id) break;

                if (!areFriendsResult.IsSuccess)
                    return Result.Failure(areFriendsResult.Exception);

                if (!areFriendsResult.Value) return ForbiddenResult();

                break;
            }
            case AccessType.Personal: {
                if (requestSender == null) return ForbiddenResult();

                if (text.AppUserId != requestSender.Id) return ForbiddenResult();

                break;
            }
            default: {
                return Result.Failure(new ServerException());
            }
        }

        return PassPasswordCheck(text, password);
    }

    public Result PassWriteSecurityChecks(Text text, AppUser requestSender, string? password) {
        var owner = text.AppUser;
        if (owner.Id != requestSender.Id) return ForbiddenResult();

        return PassPasswordCheck(text, password);
    }

    public Result PassPasswordCheck(Text text, string? password) {
        var securitySettings = text.TextSecuritySettings;
        var user = text.AppUser;

        if (securitySettings.Password == null) return Result.Success();
        
        if (password == null) return Result.Failure(new BadRequestException("Password is not provided."));

        var passwordCheck = _passwordHasher.VerifyHashedPassword(user,
            securitySettings.Password!, password);
        if (passwordCheck == PasswordVerificationResult.Failed) return Result.Failure(new BadRequestException("Provided password is not correct."));

        return Result.Success();
    }

    public string HashPassword(AppUser user, string password) {
        return _passwordHasher.HashPassword(user, password);
    }

    private Result ForbiddenResult() {
        return Result.Failure(new ForbiddenException());
    }
}