namespace Auth.Model;

public class UserDocumentRoleAssignment {
    public User User {get; set;} = default!;
    public string UserId {get; set;} = default!;
    public DocumentMetadata Text {get; set;} = default!;
    public string TextId {get; set;} = default!;
    public DocumentRole Role {get; set;} = default!;
    public string RoleId {get; set;} = default!;
}