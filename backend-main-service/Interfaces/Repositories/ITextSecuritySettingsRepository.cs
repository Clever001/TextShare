using DocShareApi.Models;

namespace DocShareApi.Interfaces.Repositories;

public interface ITextSecuritySettingsRepository {
    Task<TextSecuritySettings?> GetByTextId(string textId);
}