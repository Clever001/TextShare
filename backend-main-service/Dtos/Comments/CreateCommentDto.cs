using System.ComponentModel.DataAnnotations;

namespace DocShareApi.Dtos.Comments;

public record CreateCommentDto(
    [Required, Length(1, 1000)]
    string Content,
    long? ParentId,
    string DocumentId
);
