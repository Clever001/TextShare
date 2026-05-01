using System.ComponentModel.DataAnnotations;

namespace DocShareApi.Dtos.Comments;

public record UpdateCommentDto(
    [property: Required]
    [property: Length(1, 1000)]
    string Content
);
