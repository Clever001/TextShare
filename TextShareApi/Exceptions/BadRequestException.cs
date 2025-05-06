namespace TextShareApi.Exceptions;

public sealed class BadRequestException : ApiException {
    public override string Code { get; init; } = "BadRequest";
    public override int CodeNumber { get; init; } = 400;
    public override string Description { get; init; } = "Request contains client error.";

    public BadRequestException() { }

    public BadRequestException(string description) {
        Description = description;
    }

    public BadRequestException(List<string> details) {
        Details = details;
    }

    public BadRequestException(string description, List<string> details) {
        Description = description;
        Details = details;
    }
}