using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TextShareApi.Data;
using TextShareApi.Interfaces;
using TextShareApi.Models;
using TextShareApi.Models.Enums;
using TextShareApi.Repositories;
using TextShareApi.ResponseClasses;

namespace TextShareApi.Services;

public class TextSecurityService : ITextSecurityService {
    private readonly AppDbContext _context;
    private readonly IFriendPairsRepository _friendPairsRepo;
    private readonly PasswordHasher<AppUser> _passwordHasher;

    public TextSecurityService(AppDbContext context, 
        IFriendPairsRepository friendPairsRepo,
        PasswordHasher<AppUser> passwordHasher) {
        _context = context;
        _friendPairsRepo = friendPairsRepo;
        _passwordHasher = passwordHasher;
    }

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
                var friendsIds = await _friendPairsRepo.GetFriendsIds(ownerId);
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
    }
}