namespace Shared.ApiError;

public sealed class NotFoundApiError : IApiError {
    public NotFoundApiError() {
        Details = [];
    }

    public NotFoundApiError(string description) {
        Description = description;
        Details = [];
    }

    public string Code { get; init; } = "NotFound";
    public int CodeNumber { get; init; } = 404;
    public string Description { get; init; } = "The requested object not found.";
    public IEnumerable<string> Details { get; init; }
}