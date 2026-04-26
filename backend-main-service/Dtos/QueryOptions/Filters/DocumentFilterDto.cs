namespace DocShareApi.Dtos.QueryOptions.Filters;

public record DocumentFilterDto(
    string? Title,
    List<string>? Tags,
    DateTime? FromDate,
    DateTime? ToDate,
    string? OwnerName
);