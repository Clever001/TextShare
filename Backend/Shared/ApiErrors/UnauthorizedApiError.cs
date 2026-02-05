namespace Shared.ApiError;

public sealed class UnauthorizedApiError : IApiError {
    public string Code { get; init; } = "Unauthorized";
    public int CodeNumber { get; init; } = 401;
    public string Description { get; init; } = "Check your registration details for correctness.";
    public IEnumerable<string> Details { get; init; } = [];
}