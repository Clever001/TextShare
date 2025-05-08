using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TextShareApi.Models;

[Table("Texts")]
public class Text {
    public string Id { get; set; } = string.Empty;
    [Length(1, 70)]
    public string Title { get; set; } = string.Empty;
    [MaxLength(250)]
    public string Description { get; set; } = string.Empty;
    [MaxLength(35)]
    public string Syntax { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedOn { get; set; } = null;
    public string OwnerId { get; set; }
    public AppUser Owner { get; set; }

    public TextSecuritySettings TextSecuritySettings { get; set; }
}