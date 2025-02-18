using TextShareApi.Models;

namespace TextShareApi.Interfaces;

public interface ITextRepository {
    Task<Text> CreateText(Text text, TextSecuritySettings textSecuritySettings);
    Task<Text?> GetTextWithBackground(string textId);
    Task<bool> ContainsText(string textId);
    Task<List<Text>> GetUsersTexts(string userName);
    Task<AppUser?> GetTextOwner(string textId);
}