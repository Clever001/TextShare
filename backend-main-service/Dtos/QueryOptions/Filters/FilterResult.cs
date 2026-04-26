namespace DocShareApi.Dtos.QueryOptions.Filters;

public record FilterResult<SearchT>(
    int TotalCount,
    List<SearchT> Selection
);