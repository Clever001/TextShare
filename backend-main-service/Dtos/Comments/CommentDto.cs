namespace DocShareApi.Dtos.Comments;

public record CommentDto(
    long Id,
    string? Content,
    long? ParentId,
    string DocumentId,
    string? AuthorId,
    string? AuthorName,
    DateTime CreatedOn
);
