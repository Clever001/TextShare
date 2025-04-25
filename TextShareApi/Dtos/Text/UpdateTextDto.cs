using TextShareApi.Models.Enums;

namespace TextShareApi.Dtos.Text;

public class UpdateTextDto {
    public string? Text { get; set; }
    public AccessType? AccessType { get; set; }
    public string? Password { get; set; }
    public bool UpdatePassword { get; set; } = true;
}