using DocShareApi.Dtos.Comments;
using DocShareApi.Models;

namespace DocShareApi.Mappers;

public static class CommentMapper {
    public static CommentDto ToDto(this Comment comment) {
        return new CommentDto(
            Id: comment.Id,
            Content: comment.Content,
            ParentId: comment.ParentId,
            DocumentId: comment.DocumentId,
            AuthorId: comment.AuthorId,
            AuthorName: comment.Author?.UserName,
            CreatedOn: comment.CreatedOn
        );
    }
}
