namespace TextShareApi.Exceptions;

public interface IApiException {
    public string Code { get; init; }
    public int CodeNumber { get; init; }
    public string Description { get; init; }
    public List<string>? Details { get; init; }
}