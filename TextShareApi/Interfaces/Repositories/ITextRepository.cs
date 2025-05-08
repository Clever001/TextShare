using System.Linq.Expressions;
using TextShareApi.Models;

namespace TextShareApi.Interfaces.Repositories;

/// <summary>
///     В возвращаемые тексты включены параметры безопасности и имя пользователя.
/// </summary>
public interface ITextRepository {
    Task AddText(Text text, TextSecuritySettings textSecuritySettings);
    Task<Text?> GetText(string textId);
    Task<List<Text>> GetTexts(Expression<Func<Text, bool>> predicate, int skipCnt = 0, int? maxCnt = null);
    Task UpdateText(Text dto);
    Task<bool> DeleteText(string textId);
    Task<bool> ContainsText(string textId);
    Task<bool> ContainsText(string title, string ownerId);
}