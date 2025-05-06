namespace TextShareApi.Exceptions;

public abstract class ApiException {
    public abstract string Code { get; init; }
    public abstract int CodeNumber { get; init; }
    public abstract string Description { get; init; }
    public List<string>? Details { get; init; } = null;
}