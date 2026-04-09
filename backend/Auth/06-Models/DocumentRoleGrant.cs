namespace Auth.Model;

public class DocumentRoleGrant {
    public string Id {get; set;} = default!;
    public DocumentMetadata Document {get; set;} = default!;
    public string DocumentId {get; set;} = default!;
    public DocumentRole Role {get; set;} = default!;
    public string RoleId {get; set;} = default!;
}