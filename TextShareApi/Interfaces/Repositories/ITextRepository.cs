using TextShareApi.Models;

namespace TextShareApi.Interfaces.Repositories;

public interface ITextRepository {
    Task<Text> CreateText(Text text);
    Task<Text?> GetTextWithBackground(string textId);
    Task<Text?> GetText(string textId);
    Task<Text?> UpdateText(string textId, string content);
    Task<bool> ContainsText(string textId);
    Task<List<Text>> GetUsersTexts(string userName);
    Task<AppUser?> GetTextOwner(string textId);
}