using Microsoft.AspNetCore.Identity;
using Shared;
using TextShareApi.Dtos.Enums;
using Shared.Exceptions;
using TextShareApi.Interfaces.Services;
using TextShareApi.Models;

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

    public async Task<Result> PassReadSecurityChecks(Text text, string? requestSenderId, string? password) {
        var textSecSettings = text.TextSecuritySettings;
        if (textSecSettings == null) {
            _logger.LogError("TextSecuritySettings is null");
            return Result.Failure(new ServerException());
        }

        switch (textSecSettings.AccessType) {
            case AccessType.ByReferencePublic: {
                break;
            }
            case AccessType.ByReferenceAuthorized: {
                if (requestSenderId == null) return ForbiddenResult();

                break;
            }
            case AccessType.OnlyFriends: {
                if (requestSenderId == null) return ForbiddenResult();
                if (text.OwnerId == requestSenderId) break;
                var areFriendsResult = await _friendService.AreFriendsById(text.OwnerId, requestSenderId);
                if (!areFriendsResult.IsSuccess)
                    return Result.Failure(areFriendsResult.Exception);
                if (!areFriendsResult.Value) return ForbiddenResult();

                break;
            }
            case AccessType.Personal: {
                if (requestSenderId == null) return ForbiddenResult();

                if (text.OwnerId != requestSenderId) return ForbiddenResult();

                break;
            }
            default: {
                return Result.Failure(new ServerException());
            }
        }

        if (requestSenderId == text.OwnerId) return Result.Success();

        return PassPasswordCheck(text, password);
    }

    public Result PassWriteSecurityChecks(Text text, string requestSenderId) {
        var ownerId = text.OwnerId;
        if (ownerId != requestSenderId) return ForbiddenResult();

        return Result.Success();
    }

    private Result PassPasswordCheck(Text text, string? password) {
        var securitySettings = text.TextSecuritySettings;
        var user = text.Owner;

        if (securitySettings.Password == null) return Result.Success();

        if (password == null) return Result.Failure(new BadRequestException("Password is not provided."));

        var passwordCheck = _passwordHasher.VerifyHashedPassword(user,
            securitySettings.Password, password);
        if (passwordCheck == PasswordVerificationResult.Failed)
            return Result.Failure(new BadRequestException("Provided password is not correct."));

        return Result.Success();
    }

    public string HashPassword(AppUser user, string password) {
        return _passwordHasher.HashPassword(user, password);
    }

    private Result ForbiddenResult() {
        return Result.Failure(new ForbiddenException());
    }
}