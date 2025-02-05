using System.ComponentModel.DataAnnotations.Schema;

namespace TextShareApi.Models;

[Table("Texts")]
public class Text {
    public string Id { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedOn { get; set; } = null;
    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }
}