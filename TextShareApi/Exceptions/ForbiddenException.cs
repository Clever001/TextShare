namespace TextShareApi.Exceptions;

public sealed class ForbiddenException : ApiException {
    public override string Code { get; init; } = "Forbidden";
    public override int CodeNumber { get; init; } = 403;
    public override string Description { get; init; } = "You do not have permission to access this resource.";
}