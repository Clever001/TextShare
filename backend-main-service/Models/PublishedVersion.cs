using System.ComponentModel.DataAnnotations.Schema;

namespace DocShareApi.Models;

[Table("PublishedVersions")]
public class PublishedVersion {
    public string DocumentId { get; set; }
    public Document Document { get; set; }
    public byte[]? Content { get; set; }
    public DateTime PublishedOn { get; set; }
}
