using Microsoft.EntityFrameworkCore;
using TextShareApi.Data;
using TextShareApi.Interfaces;
using TextShareApi.Models;

namespace TextShareApi.Repositories;

public class TextRepository : ITextRepository {
    private readonly AppDbContext _context;
    /*private readonly UniqueIdService _uniqueIdService;*/

    public TextRepository(AppDbContext context/*,
        UniqueIdService uniqueIdService*/) {
        _context = context;
        // _uniqueIdService = uniqueIdService;
    }
    
    public async Task<Text> CreateText(Text text) {
        await _context.Texts.AddAsync(text);
        await _context.SaveChangesAsync();
        return text;
    }

    public async Task<Text?> GetTextById(string textId) {
        var text = await _context.Texts
            .Include(t => t.AppUser)
            .FirstOrDefaultAsync(t => t.Id == textId);
        return text;
    }
}