namespace TextShareApi.Exceptions;

public sealed class ServerException : IApiException {
    public ServerException() { }

    public ServerException(string description) {
        Description = description;
    }

    public string Code { get; init; } = "InternalServerError";
    public int CodeNumber { get; init; } = 500;
    public string Description { get; init; } = "Server side error occured.";
    public List<string>? Details { get; init; } = null;
}