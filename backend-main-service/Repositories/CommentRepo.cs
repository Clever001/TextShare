using DocShareApi.Data;
using DocShareApi.Dtos.Comments;
using DocShareApi.Dtos.QueryOptions.Filters;
using DocShareApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DocShareApi.Repositories;

public class CommentRepo(
    AppDbContext context
) : ICommentsRepo {
    public async Task<long> Create(CreateCommentCommand command, CreateCommentDto dto) {
        var newComment = new Comment() {
            Content = dto.Content,
            ParentId = dto.ParentId,
            DocumentId = dto.DocumentId,
            AuthorId = command.AuthorId,
            IsDevelopmentComment = command.IsDevelopmentComment
        };
        await context.Comments.AddAsync(newComment);
        await context.SaveChangesAsync();
        return newComment.Id;
    }

    public async Task<Comment?> GetById(long commentId) {
        return await context.Comments
            .Include(c => c.Author)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == commentId);
    }

    public async Task<bool> ContainsById(long commentId) {
        return await context.Comments
            .Where(c => c.Id == commentId)
            .AnyAsync();
    }

    public async Task<FilterResult<Comment>> GetAll<OrderT>(
        QueryFilter<Comment, OrderT> filter
    ) {
        IQueryable<Comment> comments = context.Comments;

        foreach (var predicate in filter.Predicates ?? []) {
            comments = comments.Where(predicate);
        }
        int count = await comments.CountAsync();
        comments = filter.IsAscending ?
            comments.OrderBy(filter.KeyOrder) :
            comments.OrderByDescending(filter.KeyOrder);
        comments = comments.Skip(filter.Skip).Take(filter.Take);
        var selection = await comments
            .Include(c => c.Author)
            .AsNoTracking()
            .ToListAsync();

        return new FilterResult<Comment>(count, selection);
    }

    public async Task Update(long commentId, UpdateCommentDto dto) {
        Comment? foundComment = await context.Comments
            .FindAsync(commentId);
        if (foundComment == null)
            throw new NullReferenceException("Object does not exist");
        if (foundComment.Content == null ||
            foundComment.AuthorId == null)
            throw new Exception("Cannot update cleared comment");

        foundComment.Content = dto.Content;
        context.Comments.Update(foundComment);
        await context.SaveChangesAsync();
    }

    public async Task Clear(long commentId) {
        Comment? foundComment = await context.Comments
            .FindAsync(commentId);
        if (foundComment == null)
            throw new NullReferenceException("Object does not exist");

        foundComment.Content = null;
        foundComment.AuthorId = null;
        context.Comments.Update(foundComment);
        await context.SaveChangesAsync();
    }
}
