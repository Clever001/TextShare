using TextShareApi.Models;
using TextShareApi.Models.Enums;
using TextShareApi.ResponseClasses;

namespace TextShareApi.Interfaces;

public interface ITextSecurityService {
    Task<TextSecuritySettings> ProvideTextSecuritySettings(Text text, AccessType accessType, string? password);
    Task<SecurityResponse> GetTextWithSecurityCheck(string textId, AppUser? sender, string? password);
}