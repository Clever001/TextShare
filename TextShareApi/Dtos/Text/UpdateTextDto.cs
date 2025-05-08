using TextShareApi.Models.Enums;

namespace TextShareApi.Dtos.Text;

public class UpdateTextDto {
    public string? Content { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Syntax { get; set; }
    public AccessType? AccessType { get; set; }
    public string? Password { get; set; }
    public bool UpdatePassword { get; set; } = true;
}