namespace Shared.ApiError;

public sealed class BadRequestApiError : IApiError {
    public BadRequestApiError() {
        Details = [];
    }

    public BadRequestApiError(string description) {
        Description = description;
        Details = [];
    }

    public BadRequestApiError(string description, IEnumerable<string> details) {
        Description = description;
        Details = details.ToArray();
    }

    public string Code { get; init; } = "BadRequest";
    public int CodeNumber { get; init; } = 400;
    public string Description { get; init; } = "Request contains client error.";
    public IEnumerable<string> Details { get; init; }
}