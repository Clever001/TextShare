using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TextShareApi.Data;
using TextShareApi.Interfaces.Repositories;
using TextShareApi.Models;

namespace TextShareApi.Repositories;

public class TextRepository : ITextRepository {
    private readonly AppDbContext _context;
    private readonly ILogger<TextRepository> _logger;

    public TextRepository(AppDbContext context,
        ILogger<TextRepository> logger) {
        _context = context;
        _logger = logger;
    }

    public async Task AddText(Text text, TextSecuritySettings textSecuritySettings) {
        await _context.Texts.AddAsync(text);
        await _context.TextSecuritySettings.AddAsync(textSecuritySettings);
        await _context.SaveChangesAsync();
    }

    public async Task<Text?> GetText(string textId) {
        return await _context.Texts
            .Include(t => t.TextSecuritySettings)
            .Include(t => t.Owner)
            .FirstOrDefaultAsync(t => t.Id == textId);
    }

    public async Task<List<Text>> GetTexts(Expression<Func<Text, bool>> predicate, int skipCnt = 0,
        int? maxCnt = null) {
        var query = _context.Texts
            .Include(t => t.TextSecuritySettings)
            .Include(t => t.Owner)
            .Where(predicate)
            .Skip(skipCnt);

        if (maxCnt != null) query = query.Take(maxCnt.Value);
        return await query.ToListAsync();
    }

    public async Task UpdateText(Text text) {
        _context.Texts.Update(text);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteText(string textId) {
        var text = await _context.Texts
            .Include(t => t.TextSecuritySettings)
            .FirstOrDefaultAsync(t => t.Id == textId);
        if (text == null) return false;

        var securitySettings = await _context.TextSecuritySettings.FindAsync(textId);
        
        if (securitySettings == null) {
            // Never executed. Text always has Security Settings table.
            _logger.LogError("SecuritySettings does not exist. Cannot delete");
        }

        if (securitySettings != null) _context.TextSecuritySettings.Remove(securitySettings);
        _context.Texts.Remove(text);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ContainsText(string textId) {
        return await _context.Texts.AnyAsync(t => t.Id == textId);
    }

    public async Task<bool> ContainsText(string title, string ownerId) {
        return await _context.Texts.AnyAsync(t => t.Title == title && t.OwnerId == ownerId);
    }
}