using System.ComponentModel.DataAnnotations;

namespace Auth.Model;

public class DocumentRole {
    [Length(1, 25)]
    public string Name {get; set;} = default!;
    public bool CanRead {get; set;}
    public bool CanComment {get; set;}
    public bool CanEdit {get; set;}
    public bool CanDelete {get; set;}
    public bool CanCreateRoleGrants {get; set;}
    public bool CanManageRoles {get; set;}
}