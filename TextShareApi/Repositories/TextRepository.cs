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
            .Include(t => t.Tags)
            .FirstOrDefaultAsync(t => t.Id == textId);
    }

    public async Task<(int, List<Text>)> GetTexts<T>(int skip,
        int take,
        Expression<Func<Text, T>> keyOrder,
        bool isAscending,
        List<Expression<Func<Text, bool>>>? predicates,
        bool generateCount) {
        IQueryable<Text> texts = _context.Texts
            .Include(t => t.TextSecuritySettings)
            .Include(t => t.Owner)
            .ThenInclude(u => u.FriendPairs)
            .Include(t => t.Tags);

        // Filtering
        if (predicates != null)
            foreach (var predicate in predicates)
                texts = texts.Where(predicate);

        var count = generateCount ? await texts.CountAsync() : -1;

        // Ordering
        texts = isAscending ? texts.OrderBy(keyOrder) : texts.OrderByDescending(keyOrder);

        // Pagination
        texts = texts.Skip(skip).Take(take);

        return (count, await texts.Select(t => new Text {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            Syntax = t.Syntax,
            Content = "", // No Content
            CreatedOn = t.CreatedOn,
            UpdatedOn = t.UpdatedOn,
            OwnerId = t.OwnerId,
            Owner = new AppUser {
                Id = t.OwnerId,
                UserName = t.Owner.UserName
            },
            Tags = t.Tags,
            TextSecuritySettings = t.TextSecuritySettings,
            ExpiryDate = t.ExpiryDate
        }).ToListAsync());
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

        if (securitySettings == null)
            // Never executed. Text always has Security Settings table.
            _logger.LogError("SecuritySettings does not exist. Cannot delete");

        if (securitySettings != null) _context.TextSecuritySettings.Remove(securitySettings);
        _context.Texts.Remove(text);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<int> DeleteExpiredTexts() {
        var now = DateTime.UtcNow;
        var expiredTexts = await _context.Texts
            .Include(t => t.TextSecuritySettings)
            .Where(t => t.ExpiryDate <= now)
            .ToListAsync();

        if (expiredTexts.Count == 0) return 0;

        var expiredTextSettings = expiredTexts.Select(t => t.TextSecuritySettings).ToList();
        _context.TextSecuritySettings.RemoveRange(expiredTextSettings);
        _context.Texts.RemoveRange(expiredTexts);
        await _context.SaveChangesAsync();
        return expiredTexts.Count;
    }

    public async Task<bool> ContainsText(string textId) {
        return await _context.Texts.AnyAsync(t => t.Id == textId);
    }

    public async Task<bool> ContainsText(string title, string ownerId) {
        return await _context.Texts.AnyAsync(t => t.Title == title && t.OwnerId == ownerId);
    }
}