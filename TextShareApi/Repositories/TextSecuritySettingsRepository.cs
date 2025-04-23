using TextShareApi.Data;
using TextShareApi.Interfaces.Repositories;
using TextShareApi.Models;

namespace TextShareApi.Repositories;

public class TextSecuritySettingsRepository : ITextSecuritySettingsRepository {
    private readonly AppDbContext _context;

    public TextSecuritySettingsRepository(AppDbContext context) {
        _context = context;
    }
    
    public async Task<TextSecuritySettings?> GetByTextId(string textId) {
        return await _context.TextSecuritySettings.FindAsync(textId);
    }
}