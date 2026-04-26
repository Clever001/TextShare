using System.ComponentModel.DataAnnotations;

namespace DocShareApi.Dtos.QueryOptions;

public sealed class PaginationDto {
    [Range(1, int.MaxValue)] public int PageNumber { get; init; }

    [Range(1, 50)] public int PageSize { get; init; }
}
