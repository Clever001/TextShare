using System.ComponentModel.DataAnnotations;

namespace DocShareApi.Dtos.QueryOptions.Filters;

public record CommentFilterDto(
    long? ParentId,
    [property: Required]
    string DocumentId
);
