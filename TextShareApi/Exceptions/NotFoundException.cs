namespace TextShareApi.Exceptions;

public sealed class NotFoundException : ApiException {
    public override string Code { get; init; } = "NotFound";
    public override int CodeNumber { get; init; } = 404;
    public override string Description { get; init; } = "The requested object not found.";

    public NotFoundException() { }

    public NotFoundException(string description) {
        Description = description;
    }
}