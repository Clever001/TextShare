using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using DocShareApi.Data;
using DocShareApi.Interfaces.Repositories;
using DocShareApi.Models;

namespace DocShareApi.Repositories;

public class TagRepository : ITagRepository {
    private readonly AppDbContext _context;

    public TagRepository(AppDbContext appDbContext) {
        _context = appDbContext;
    }

    public async Task<List<Tag>> GetTags(Expression<Func<Tag, bool>> predicate) {
        return await _context.Tags.Where(predicate).ToListAsync();
    }
}