using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TextShareApi.Data;
using TextShareApi.Interfaces.Repositories;
using TextShareApi.Models;

namespace TextShareApi.Repositories;

public class TagRepository : ITagRepository {
    private readonly AppDbContext _context;

    public TagRepository(AppDbContext appDbContext) {
        _context = appDbContext;
    }

    public async Task<List<Tag>> GetTags(Expression<Func<Tag, bool>> predicate) {
        return await _context.Tags.Where(predicate).ToListAsync();
    }
}