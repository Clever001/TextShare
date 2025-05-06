namespace TextShareApi.Dtos.Additional;

public class ExceptionDto {
    public string Code { get; set; } = "";
    public string Description { get; set; } = "";
    public List<string>? Details { get; set; } = null;

    public static ExceptionDto ServerError() {
        return new ExceptionDto {
            Code = "ServerError",
            Description = "A server error occurred."
        };
    }
}