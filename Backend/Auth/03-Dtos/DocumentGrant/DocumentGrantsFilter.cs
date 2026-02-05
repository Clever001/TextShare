namespace Auth.Dto.DocumentGrant;

public class DocumentGrantsFilter {
    public string? DocumentId {get; init;}
    public string? RoleName {get; init;}
    public string CallingUserId {get; init;} = default!;
}