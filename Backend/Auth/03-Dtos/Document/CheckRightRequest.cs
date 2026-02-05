namespace Auth.Dto.Document;

public class CheckRightRequest {
    public string DocumentId {get; init;} = default!;
    public string CallingUserId {get; init;} = default!;
}