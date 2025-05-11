namespace TextShareApi.Dtos.Exception;

public sealed class ExceptionDto {
    public string Code { get; init; } = "";
    public string Description { get; init; } = "";
    public List<string>? Details { get; init; }
}