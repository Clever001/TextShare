namespace Shared.ApiError;

public sealed class ForbiddenApiError : IApiError {
    public string Code { get; init; } = "Forbidden";
    public int CodeNumber { get; init; } = 403;
    public string Description { get; init; } = "You do not have permission to access this resource.";
    public IEnumerable<string> Details { get; init; } = [];
}