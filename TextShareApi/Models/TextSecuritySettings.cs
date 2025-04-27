using TextShareApi.Models.Enums;

namespace TextShareApi.Models;

public class TextSecuritySettings {
    public string TextId { get; set; } = null!;
    public Text Text { get; set; } = null!;

    public AccessType AccessType { get; set; }
    public string? Password { get; set; } = null;
}