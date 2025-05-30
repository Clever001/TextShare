using System.ComponentModel.DataAnnotations;
using TextShareApi.Attributes;
using TextShareApi.Dtos.Enums;

namespace TextShareApi.Dtos.Text;

public class UpdateTextDto {
    public string? Content { get; set; }

    [MaxLength(70)] public string? Title { get; set; }

    [MaxLength(250)] public string? Description { get; set; }

    [MaxLength(35)] public string? Syntax { get; set; }

    public List<string>? Tags { get; set; }
    public AccessType? AccessType { get; set; }
    public string? Password { get; set; }
    public bool UpdatePassword { get; set; } = true;

    [ExpiryDate] public DateTime? ExpiryDate { get; set; }
}