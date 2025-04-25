using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TextShareApi.ClassesLib;
using TextShareApi.Data;
using TextShareApi.Interfaces.Repositories;
using TextShareApi.Interfaces.Services;
using TextShareApi.Models;
using TextShareApi.Models.Enums;

namespace TextShareApi.Services;

public class TextSecurityService : ITextSecurityService {
    private readonly AppDbContext _context;
    private readonly IFriendPairRepository _friendPairRepo;
    private readonly PasswordHasher<AppUser> _passwordHasher;
    private readonly IFriendService _friendService;

    public TextSecurityService(AppDbContext context, 
        IFriendPairRepository friendPairRepo,
        PasswordHasher<AppUser> passwordHasher,
        IFriendService friendService) {
        _context = context;
        _friendPairRepo = friendPairRepo;
        _passwordHasher = passwordHasher;
        _friendService = friendService;
    }

    /*
    public async Task<TextSecuritySettings> ProvideTextSecuritySettings(Text text, AccessType accessType, string? password) {
        var settings = new TextSecuritySettings {
            TextId = text.Id,
            AccessType = accessType,
            Password = password,
        };
        await _context.TextSecuritySettings.AddAsync(settings);
        await _context.SaveChangesAsync();
        return settings;
    }

    public async Task<SecurityResponse> GetTextWithSecurityCheck(string textId, AppUser? sender, string? password) {
        
        Text? text = await _context.Texts
            .Include(t => t.AppUser)
            .Include(t => t.TextSecuritySettings)
            .FirstOrDefaultAsync(t => t.Id == textId);
        if (text is null) return new(SecurityCheckResult.NotFound);
        
        if (sender is not null && sender.Id == text.AppUserId) {
            return new(SecurityCheckResult.Allowed, text); // Owner always has rights.
        }
        
        var textSecuritySettings = text.TextSecuritySettings;
        switch (textSecuritySettings.AccessType) {
            case AccessType.Personal: {
                if (sender is null) return new(SecurityCheckResult.Forbidden);
                if (sender.Id != text.AppUserId) return new (SecurityCheckResult.Forbidden);
                break;
            }
            case AccessType.ByReferencePublic: {
                break;
            }
            case AccessType.ByReferenceAuthorized: {
                if (sender is null) return new(SecurityCheckResult.Forbidden);
                break;
            }
            case AccessType.OnlyFriends: {
                if (sender is null) return new(SecurityCheckResult.Forbidden);
                var ownerId = text.AppUserId;
                var friendsIds = await _friendPairRepo.GetFriendsIds(ownerId);
                if (!friendsIds.Contains(sender.Id)) return new(SecurityCheckResult.Forbidden);
                break;
            }
        }

        if (textSecuritySettings.Password is not null) {
            if (password is null) return new (SecurityCheckResult.NoPasswordProvided);
            var result = _passwordHasher.VerifyHashedPassword(text.AppUser, textSecuritySettings.Password,
                password);
            if (result == PasswordVerificationResult.Failed) {
                return new (SecurityCheckResult.PasswordIsNotValid);
            }
        }

        return new (SecurityCheckResult.Allowed, text);
    }*/

    public async Task<Result> PassSecurityChecks(Text text, AppUser? appUser, string? password) {
        var textSecSettings = text.TextSecuritySettings;
        Debug.Assert(textSecSettings != null);

        switch (textSecSettings.AccessType) {
            case AccessType.ByReferencePublic: {
                return Result.Success();
            }
            case AccessType.ByReferenceAuthorized: {
                if (appUser == null) {
                    return Result.Failure("You need to be authorized to access this text.", true);
                }

                return Result.Success();
            }
            case AccessType.OnlyFriends: {
                if (appUser == null) {
                    return Result.Failure("You need to be authorized to access this text.", true);
                }

                var areFriendsResult = await _friendService.AreFriends(text.AppUser.UserName!, appUser.UserName!);
                
                if (!areFriendsResult.IsSuccess) {
                    return Result.Failure(areFriendsResult.Error, areFriendsResult.IsClientError);
                }

                if (!areFriendsResult.Value) {
                    return Result.Failure("You don't have enough rights to view this text.", true);
                }

                return Result.Success();
            }
            case AccessType.Personal: {
                if (appUser == null) {
                    return Result.Failure("You need to be authorized to access this text.", true);
                }

                if (text.AppUserId != appUser.Id) {
                    return Result.Failure("You don't have enough rights to view this text.", true);
                }
                
                return Result.Success();
            }
            default: {
                return Result.Failure("This text has unknown access type.", false);
            }
        }
    }
}