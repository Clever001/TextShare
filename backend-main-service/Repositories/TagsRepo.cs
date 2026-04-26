using System.Diagnostics;
using System.Linq.Expressions;
using DocShareApi.Data;
using DocShareApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DocShareApi.Repositories;

public class TagsRepo(
    AppDbContext dbContext
) : ITagRepo {
    public async Task CreateTags(List<Tag> tags) {
        await dbContext.Tags.AddRangeAsync(tags);
        await dbContext.SaveChangesAsync();
    }

    public async Task<List<Tag>> GetTags(Expression<Func<Tag, bool>> predicate) {
        return await dbContext.Tags.Where(predicate).ToListAsync();
    }

    public async Task RemoveTags(List<Tag> tags) {
        dbContext.Tags.RemoveRange(tags);
        await dbContext.SaveChangesAsync();
    }
}