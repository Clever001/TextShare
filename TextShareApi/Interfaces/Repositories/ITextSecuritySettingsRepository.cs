using TextShareApi.Models;

namespace TextShareApi.Interfaces;

public interface ITextSecuritySettingsRepository {
    Task<TextSecuritySettings?> GetByTextId(string textId);
}