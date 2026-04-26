using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocShareApi.Models;

[Table("Tags")]
public class Tag : IEquatable<Tag> {
    [Length(1, 20)] public string Name { get; init; } = string.Empty;
    public List<Document> Documents { get; init; } = new();


    public bool Equals(Tag? other) {
        if (other == null) return false;
        return Name.Equals(other.Name);
    }

    public override int GetHashCode() {
        return Name.GetHashCode();
    }
}
