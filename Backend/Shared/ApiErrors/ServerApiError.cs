namespace Shared.ApiError;

public sealed class ServerApiError : IApiError {
    public ServerApiError() {
        Details = [];
    }

    public ServerApiError(string description) {
        Description = description;
        Details = [];
    }

    public string Code { get; init; } = "InternalServerError";
    public int CodeNumber { get; init; } = 500;
    public string Description { get; init; } = "Server side error occured.";
    public IEnumerable<string> Details { get; init; }
}