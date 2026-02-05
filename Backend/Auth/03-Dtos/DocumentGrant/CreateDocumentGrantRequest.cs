namespace Auth.Dto.DocumentGrant;

public class CreateDocumentGrantRequest {
    public string DocumentId {get; init;} = default!;
    public string RoleName {get; init;} = default!;
    public string CallingUserId {get; init;} = default!;
}