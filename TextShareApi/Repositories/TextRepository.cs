using Microsoft.EntityFrameworkCore;
using TextShareApi.Data;
using TextShareApi.Interfaces.Repositories;
using TextShareApi.Models;

namespace TextShareApi.Repositories;

public class TextRepository : ITextRepository {
    private readonly AppDbContext _context;
    public TextRepository(AppDbContext context) {
        _context = context;
    }
    
    public async Task<Text> CreateText(Text text) {
        await _context.Texts.AddAsync(text);
        await _context.SaveChangesAsync();
        return text;
    }

    public async Task<Text?> GetTextWithBackground(string textId) {
        return await _context.Texts
            .Include(t => t.AppUser)
            .Include(t => t.TextSecuritySettings)
            .FirstOrDefaultAsync(t => t.Id == textId);
    }

    public async Task<Text?> GetText(string textId) {
        return await _context.Texts
            .FindAsync(textId);
    }

    public async Task<Text?> UpdateText(string textId, string content) {
        var text = await _context.Texts.FindAsync(textId);
        if (text is null) return null;

        text.Content = content;
        text.UpdatedOn = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return text;
    }

    public async Task<bool> ContainsText(string textId) {
        return await _context.Texts.AnyAsync(t => t.Id == textId);
    }

    public async Task<List<Text>> GetUsersTexts(string userName) {
        var texts = await _context.Texts
            .Include(t => t.AppUser)
            .Where(t => t.AppUser.UserName == userName)
            .ToListAsync();
        return texts;
    }

    public async Task<AppUser?> GetTextOwner(string textId) {
        var text = await _context.Texts.Include(t => t.AppUser)
            .FirstOrDefaultAsync(t => t.Id == textId);
        if (text is null) return null;
        return text.AppUser;
    }
}