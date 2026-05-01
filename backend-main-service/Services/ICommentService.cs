using DocShareApi.ClassesLib;
using DocShareApi.Dtos.Comments;
using DocShareApi.Dtos.QueryOptions;
using DocShareApi.Dtos.QueryOptions.Filters;
using DocShareApi.Models;

namespace DocShareApi.Services;

public interface ICommentService {
    Task<Result<Comment>> CreateComment(string callerId, CreateCommentDto dto);
    Task<Result<Comment>> GetComment(long commentId);
    Task<Result<PaginatedResponseDto<Comment>>> SearchComments(
        PaginationDto pagination, CommentFilterDto commentFilter
    );
    Task<Result<Comment>> UpdateComment(
        string callerId, long commentId, UpdateCommentDto dto
    );
    Task<Result> ClearComment(string callerId, long commentId);
}
