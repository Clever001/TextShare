namespace TextShareApi.Exceptions;

public sealed class UnauthorizedException : ApiException {
    public override string Code { get; init; } = "Unauthorized";
    public override int CodeNumber { get; init; } = 401;
    public override string Description { get; init; } = "Check your registration details for correctness.";
}