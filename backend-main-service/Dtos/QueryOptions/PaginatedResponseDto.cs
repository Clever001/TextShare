namespace TextShareApi.Dtos.QueryOptions;

public sealed class PaginatedResponseDto<T> {
    public List<T> Items { get; init; } = new();
    public int TotalItems { get; init; }
    public int TotalPages { get; init; }
    public int CurrentPage { get; init; }
    public int PageSize { get; init; }
}