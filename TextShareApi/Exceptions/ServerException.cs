namespace TextShareApi.Exceptions;

public sealed class ServerException : ApiException {
    public override string Code { get; init; } = "InternalServerError";
    public override int CodeNumber { get; init; } = 500;
    public override string Description { get; init; } = "Server side error occured.";
}