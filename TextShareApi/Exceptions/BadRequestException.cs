namespace TextShareApi.Exceptions;

public sealed class BadRequestException : IApiException {
    public BadRequestException() { }

    public BadRequestException(string description) {
        Description = description;
    }

    public BadRequestException(string description, List<string> details) {
        Description = description;
        Details = details;
    }

    public string Code { get; init; } = "BadRequest";
    public int CodeNumber { get; init; } = 400;
    public string Description { get; init; } = "Request contains client error.";
    public List<string>? Details { get; init; }
}