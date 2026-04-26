using System.ComponentModel.DataAnnotations.Schema;
using DocShareApi.Dtos.Enums;

namespace DocShareApi.Models;

[Table("UserToDocRoles")]
public class UserToDocRole {
    public string UserId { get; set; }
    public AppUser User { get; set; }
    public string DocumentId { get; set; }
    public Document Document { get; set; }
    public UserDevRole Role {get; set;}
}
