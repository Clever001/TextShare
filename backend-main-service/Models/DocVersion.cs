using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocShareApi.Models;

[Table("Versions")]
public class DocVersion {
    public long Id { get; set; }
    public string DocumentId { get; set; }
    public Document Document { get; set; }
    public byte[]? Content { get; set; }
    public DateTime CreatedOn { get; set; }
    [Length(1, 70)] public string Name { get; set; }
}
