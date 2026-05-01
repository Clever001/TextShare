using DocShareApi.ClassesLib;
using DocShareApi.Dtos.Comments;
using DocShareApi.Dtos.QueryOptions;
using DocShareApi.Dtos.QueryOptions.Filters;
using DocShareApi.Models;

namespace DocShareApi.Services;

public interface ICommentService {
    Task<Result<CommentDto>> CreateComment(string callerId, CreateCommentDto dto);
    Task<Result<CommentDto>> GetComment(long commentId);
    Task<Result<PaginatedResponseDto<CommentDto>>> SearchComments(
        PaginationDto pagination, CommentFilterDto commentFilter
    );
    Task<Result<CommentDto>> UpdateComment(
        string callerId, long commentId, UpdateCommentDto dto
    );
    Task<Result> ClearComment(string callerId, long commentId);
}
