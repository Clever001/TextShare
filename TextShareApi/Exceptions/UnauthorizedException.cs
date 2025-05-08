namespace TextShareApi.Exceptions;

public sealed class UnauthorizedException : IApiException {
    public string Code { get; init; } = "Unauthorized";
    public int CodeNumber { get; init; } = 401;
    public string Description { get; init; } = "Check your registration details for correctness.";
    public List<string>? Details { get; init; } = null;
}