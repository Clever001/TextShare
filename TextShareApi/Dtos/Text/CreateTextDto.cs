using TextShareApi.Dtos.Enums;

namespace TextShareApi.Dtos.Text;

public class CreateTextDto {
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Syntax { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new List<string>();
    public AccessType AccessType { get; set; } = AccessType.Personal;
    public string? Password { get; set; } = null;
}