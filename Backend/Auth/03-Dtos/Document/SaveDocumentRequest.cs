namespace Auth.Dto.Document;

public class SaveDocumentRequest {
    public string DocumentId {get; init;} = default!;
    public string CreatorId {get; init;} = default!;
    public string DefaultRoleName {get; init;} = default!;
}