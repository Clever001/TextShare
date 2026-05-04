using System.ComponentModel.DataAnnotations;

namespace DocShareApi.Dtos.QueryOptions;

public sealed class PaginatedResponseDto<T> {
    [Required]
    public List<T> Items { get; init; } = new();
    [Required]
    public int TotalItems { get; init; }
    [Required]
    public int TotalPages { get; init; }
    [Required]
    public int CurrentPage { get; init; }
    [Required]
    public int PageSize { get; init; }
}
