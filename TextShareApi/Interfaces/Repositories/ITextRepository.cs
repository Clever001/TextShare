using System.Linq.Expressions;
using TextShareApi.Models;

namespace TextShareApi.Interfaces.Repositories;

/// <summary>
///     В возвращаемые тексты включены параметры безопасности и имя пользователя.
/// </summary>
public interface ITextRepository {
    Task AddText(Text text, TextSecuritySettings textSecuritySettings);
    Task<Text?> GetText(string textId);
    Task<(int, List<Text>)> GetTexts<T>(int skip,
        int take,
        Expression<Func<Text, T>> keyOrder,
        bool isAscending,
        List<Expression<Func<Text, bool>>>? predicates,
        bool generateCount);
    Task UpdateText(Text dto);
    Task<bool> DeleteText(string textId);
    Task<bool> ContainsText(string textId);
    Task<bool> ContainsText(string title, string ownerId);
}