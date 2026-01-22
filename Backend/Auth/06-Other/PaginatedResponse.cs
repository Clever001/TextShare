namespace Auth.Other;

public sealed class PaginatedResponse<T> {
    public List<T> Items { get; init; } = new();
    public int TotalItems { get; init; }
    public int TotalPages { get; init; }
    public int CurrentPage { get; init; }
    public int PageSize { get; init; }
}