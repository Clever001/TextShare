using TextShareApi.Attributes;

namespace TextShareApi.Dtos.QueryOptions;

public sealed class SortDto {
    public string SortBy { get; init; } = string.Empty;
    public bool SortAscending { get; init; }
}