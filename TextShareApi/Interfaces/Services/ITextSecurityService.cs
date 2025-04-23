using TextShareApi.ClassesLib;
using TextShareApi.Models;
using TextShareApi.Models.Enums;

namespace TextShareApi.Interfaces.Services;

public interface ITextSecurityService {
    Task<TextSecuritySettings> ProvideTextSecuritySettings(Text text, AccessType accessType, string? password);
    Task<SecurityResponse> GetTextWithSecurityCheck(string textId, AppUser? sender, string? password);
}