using System.Linq.Expressions;
using DocShareApi.ClassesLib;
using DocShareApi.Dtos.Comments;
using DocShareApi.Dtos.QueryOptions;
using DocShareApi.Dtos.QueryOptions.Filters;
using DocShareApi.Exceptions;
using DocShareApi.Mappers;
using DocShareApi.Models;
using DocShareApi.Repositories;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Perfolizer.Mathematics.Cpd;

namespace DocShareApi.Services;

using RC = Result<Comment>;
using RPC = Result<PaginatedResponseDto<Comment>>;
using R = Result;

public class CommentService(
    ICommentsRepo commentsRepo,
    IAccountRepository accRepo,
    IDocumentRepo docRepo,
    ILogger<CommentService> logger
) : ICommentService {
    public async Task<RC> CreateComment(string callerId, CreateCommentDto dto) {
        if (!await accRepo.ContainsAccountById(callerId)) {
            return RC.Failure(
                new BadRequestException("User with such id does not exist")
            );
        }
        if (!await docRepo.ContainsById(dto.DocumentId)) {
            return RC.Failure(
                new BadRequestException("Document with such id does not exist")
            );
        }
        if (dto.ParentId != null) {
            Comment? parent = await commentsRepo.GetById(dto.ParentId.Value);
            if (parent == null) {
                return RC.Failure(
                    new BadRequestException("Comment with such ParentId does not exist")
                );
            }
            if (parent.DocumentId != dto.DocumentId) {
                return RC.Failure(
                    new BadRequestException("Parent DocumentId doesn't match new comment DocumentId")
                );
            }
        }

        var createCommand = new CreateCommentCommand(
            callerId, false, DateTime.UtcNow
        );

        try {
            long commentId = await commentsRepo.Create(createCommand, dto);
            Comment? newComment = await commentsRepo.GetById(commentId)
                ?? throw new NullReferenceException("Couldn't find created comment.");
            return RC.Success(newComment);
        } catch (Exception ex) {
            logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            return RC.Failure(new ServerException());
        }
    }

    public async Task<RC> GetComment(long commentId) {
        Comment? comment = await commentsRepo.GetById(commentId);
        if (comment == null ||
            comment.IsDevelopmentComment == true) {
            return RC.Failure(
                new NotFoundException()
            );
        }
        return RC.Success(comment);
    }

    public async Task<RPC> SearchComments(
        PaginationDto pagination, CommentFilterDto filter
    ) {
        // Pagination
        var (skip, take) = pagination.ToSkipAndTake();

        // Filtering
        var predicates = new List<Expression<Func<Comment, bool>>> {
            c => c.ParentId == filter.ParentId,
            c => c.DocumentId == filter.DocumentId
        };

        var result = await commentsRepo.GetAll(new QueryFilter<Comment, DateTime>(
            skip, take, c => c.CreatedOn, true, predicates
        ));

        return RPC.Success(
            result.ToPaginatedResponse(pagination)
        );
    }

    public async Task<RC> UpdateComment(string callerId, long commentId, UpdateCommentDto dto) {
        if (!await accRepo.ContainsAccountById(callerId)) {
            return RC.Failure(
                new BadRequestException("User with such id does not exist")
            );
        }

        Comment? comment = await commentsRepo.GetById(commentId);
        if (comment == null) {
            return RC.Failure(new NotFoundException());
        }
        if (comment.AuthorId != callerId) {
            return RC.Failure(
                new ForbiddenException()
            );
        }

        try {
            await commentsRepo.Update(commentId, dto);
            Comment? newComment = await commentsRepo.GetById(commentId)
                ?? throw new NullReferenceException("Couldn't find created comment.");
            return RC.Success(newComment);
        } catch (Exception ex) {
            logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            return RC.Failure(new ServerException());
        }
    }

    public async Task<R> ClearComment(string callerId, long commentId) {
        if (!await accRepo.ContainsAccountById(callerId)) {
            return R.Failure(
                new BadRequestException("User with such id does not exist")
            );
        }

        Comment? comment = await commentsRepo.GetById(commentId);
        if (comment == null) {
            return R.Failure(new NotFoundException());
        }
        if (comment.AuthorId != callerId) {
            return R.Failure(
                new ForbiddenException()
            );
        }

        try {
            await commentsRepo.Clear(commentId);
            return R.Success();
        } catch (Exception ex) {
            logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            return R.Failure(new ServerException());
        }
    }
}
