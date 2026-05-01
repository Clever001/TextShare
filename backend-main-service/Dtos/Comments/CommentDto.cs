namespace DocShareApi.Dtos.Comments;

public record CommentDto(
    long Id,
    string? Content,
    long? ParentId,
    string DocumentId,
    string? AuthorId,
    string? AuthorName,
    bool HasChildren,
    bool IsDevelopmentComment,
    DateTime CreatedOn
);
