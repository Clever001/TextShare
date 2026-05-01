namespace DocShareApi.Dtos.Comments;

public record CreateCommentCommand(
    string AuthorId,
    bool IsDevelopmentComment,
    DateTime CreatedOn
);
