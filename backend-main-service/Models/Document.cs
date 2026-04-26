using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.Xml;

namespace DocShareApi.Models;

[Table("Documents")]
public class Document {
    public string Id { get; set; } = string.Empty;

    [Length(1, 70)] public string Title { get; set; } = string.Empty;
    [MaxLength(250)] public string Description { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public string OwnerId { get; set; }
    public AppUser Owner { get; set; }
    public List<Tag> Tags { get; set; } = new();
    public List<UserToDocRole> UserRoles { get; set; } = new();
    public List<DocVersion> Versions { get; set; } = new();
    public PublishedVersion? PublishedVersion { get; set; } = null;
    public List<Comment> Comments { get; set; } = new();
}
