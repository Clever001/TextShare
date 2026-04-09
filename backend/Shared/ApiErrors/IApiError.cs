namespace Shared.ApiError;

public interface IApiError {
    string Code { get; init; }
    int CodeNumber { get; init; }
    string Description { get; init; }
    IEnumerable<string> Details { get; init; }
}