namespace TextShareApi.Exceptions;

public sealed class NotFoundException : IApiException {
    public NotFoundException() { }

    public NotFoundException(string description) {
        Description = description;
    }

    public string Code { get; init; } = "NotFound";
    public int CodeNumber { get; init; } = 404;
    public string Description { get; init; } = "The requested object not found.";
    public List<string>? Details { get; init; } = null;
}