using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TextShareApi.Data;
using TextShareApi.Dtos.Text;
using TextShareApi.Interfaces.Repositories;
using TextShareApi.Models;

namespace TextShareApi.Repositories;

public class TextRepository : ITextRepository {
    private readonly AppDbContext _context;
    public TextRepository(AppDbContext context) {
        _context = context;
    }

    public async Task AddText(Text text, TextSecuritySettings textSecuritySettings) {
        await _context.Texts.AddAsync(text);
        await _context.TextSecuritySettings.AddAsync(textSecuritySettings);
        await _context.SaveChangesAsync();
    }
    
    public async Task<Text?> GetText(string textId) {
        return await _context.Texts
            .Include(t => t.TextSecuritySettings)
            .Include(t => t.AppUser)
            .FirstOrDefaultAsync(t => t.Id == textId);
    }

    public async Task<List<Text>> GetTexts(Expression<Func<Text, bool>> predicate, int skipCnt = 0, int? maxCnt = null) {
        var query = _context.Texts
            .Include(t => t.TextSecuritySettings)
            .Include(t => t.AppUser)
            .Where(predicate)
            .Skip(skipCnt);

        if (maxCnt != null) {
            query = query.Take(maxCnt.Value);
        }
        return await query.ToListAsync();
    }

    public async Task<Text?> UpdateText(string textId, UpdateTextDto dto) {
        var text = await _context.Texts
            .Include(t => t.TextSecuritySettings)
            .FirstOrDefaultAsync(t => t.Id == textId);

        if (text == null) {
            return null;
        }

        if (dto.Text != null) {
            text.Content = dto.Text;
        }
        if (dto.AccessType != null) {
            text.TextSecuritySettings.AccessType = dto.AccessType.Value;
        }

        if (dto.UpdatePassword) {
            text.TextSecuritySettings.Password = dto.Password;
        }

        await _context.SaveChangesAsync();
        
        return text;
    }

    public async Task<bool> DeleteText(string textId) {
        var text = await _context.Texts
            .Include(t => t.TextSecuritySettings)
            .FirstOrDefaultAsync(t => t.Id == textId);

        if (text == null) {
            return false;
        }

        var securitySettings = await _context.TextSecuritySettings.FindAsync(textId);
        
        Debug.Assert(securitySettings != null, "SecuritySettings does not exist. Cannot delete");
        
        if (securitySettings != null) _context.TextSecuritySettings.Remove(securitySettings);
        _context.Texts.Remove(text);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ContainsText(string textId) {
        return await _context.Texts.AnyAsync(t => t.Id == textId);
    }
}