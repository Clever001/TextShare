namespace Auth.Dto.DocumentGrant;

public class DeleteDocumentGrantRequest {
    public string DocumentGrantId {get; init;} = default!;
    public string CallingUserId {get; init;} = default!;
}