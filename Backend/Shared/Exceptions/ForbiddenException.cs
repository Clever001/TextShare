namespace Shared.Exceptions;

public sealed class ForbiddenException : IApiException {
    public string Code { get; init; } = "Forbidden";
    public int CodeNumber { get; init; } = 403;
    public string Description { get; init; } = "You do not have permission to access this resource.";
    public List<string>? Details { get; init; } = null;
}