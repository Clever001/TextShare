using DocShareApi.Dtos.Enums;

namespace DocShareApi.Dtos.QueryOptions.Filters;

public sealed class TextFilterDto {
    public string? OwnerName { get; init; }
    public string? Title { get; init; }
    public List<string>? Tags { get; init; }
    public string? Syntax { get; init; }
    public AccessType? AccessType { get; init; }
    public bool? HasPassword { get; init; }
}