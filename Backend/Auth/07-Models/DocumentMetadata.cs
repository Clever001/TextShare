namespace Auth.Model;

public class DocumentMetadata {
    public string Id {get; set;} = default!;
    public User Owner {get; set;} = default!;
    public string OwnerId {get; set;} = default!;
    public DocumentRole DefaultRole {get; set;} = default!;
    public string DefaultRoleId {get; set;} = default!;

    public List<DocumentRoleGrant> RoleGrants {get; set;} = default!;
}