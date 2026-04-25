using TextShareApi.Models;

namespace TextShareApi.Interfaces.Repositories;

public interface ITextSecuritySettingsRepository {
    Task<TextSecuritySettings?> GetByTextId(string textId);
}