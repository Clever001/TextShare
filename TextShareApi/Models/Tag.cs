using System.ComponentModel.DataAnnotations;

namespace TextShareApi.Models;

public class Tag : IEquatable<Tag> {
    [Length(1, 30)] public string Name { get; init; } = string.Empty;

    public List<Text> Texts { get; init; } = new();


    public bool Equals(Tag? other) {
        if (other == null) return false;
        return Name.Equals(other.Name);
    }

    public override int GetHashCode() {
        return Name.GetHashCode();
    }
}