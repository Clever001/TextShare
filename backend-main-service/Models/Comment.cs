using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocShareApi.Models;

[Table("Comments")]
public class Comment {
    public long Id { get; set; }
    [Length(1, 1000)] public string? Content { get; set; }
    public long? ParentId { get; set; }
    public Comment? Parent { get; set; }
    public List<Comment> Children { get; set; } = [];
    public string DocumentId { get; set; } = "";
    public Document Document { get; set; } = null!;
    public string? AuthorId { get; set; }
    public AppUser? Author { get; set; }
    public DateTime CreatedOn { get; set; }
    public bool IsDevelopmentComment { get; set; } = true;
}
