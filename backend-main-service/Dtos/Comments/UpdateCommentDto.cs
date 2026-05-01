using System.ComponentModel.DataAnnotations;

namespace DocShareApi.Dtos.Comments;

public record UpdateCommentDto(
    [Required, Length(1, 1000)]
    string Content
);
