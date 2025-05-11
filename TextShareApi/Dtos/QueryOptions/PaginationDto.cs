using System.ComponentModel.DataAnnotations;

namespace TextShareApi.Dtos.QueryOptions;

public sealed class PaginationDto {
    [Range(1, int.MaxValue)]
    public int PageNumber { get; init; }
    [Range(1, 20)]
    public int PageSize { get; init; }
}