using DocShareApi.Dtos.Comments;
using DocShareApi.Dtos.QueryOptions.Filters;
using DocShareApi.Models;

namespace DocShareApi.Repositories;

public interface ICommentsRepo {
    Task<long> Create(CreateCommentCommand command, CreateCommentDto dto);
    Task<Comment?> GetById(long commentId);
    Task<bool> ContainsById(long commentId);
    Task<FilterResult<Comment>> GetAll<OrderT>(
        QueryFilter<Comment, OrderT> filter
    );
    Task Update(long commentId, UpdateCommentDto dto);
    Task Clear(long commentId);
}
