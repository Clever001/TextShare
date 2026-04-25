using DocShareApi.Dtos.Enums;

namespace DocShareApi.Dtos.QueryOptions.Filters;

public sealed class TextFilterWithoutOwnerDto {
    public string? Title { get; init; }
    public List<string>? Tags { get; init; }
    public string? Syntax { get; init; }
    public AccessType? AccessType { get; init; }
    public bool? HasPassword { get; init; }
}