using System.ComponentModel.DataAnnotations;

namespace DocShareApi.Dtos.QueryOptions.Filters;

public record CommentFilterDto(
    long? ParentId,
    [Required]
    string DocumentId
);
