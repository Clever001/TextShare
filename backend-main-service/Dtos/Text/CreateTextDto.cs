using System.ComponentModel.DataAnnotations;
using DocShareApi.Attributes;
using DocShareApi.Dtos.Enums;
namespace DocShareApi.Dtos.Text;

public class CreateTextDto {
    [Required] [MaxLength(70)] public string Title { get; set; } = string.Empty;

    [MaxLength(250)] public string Description { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    [Required] [MaxLength(35)] public string Syntax { get; set; } = string.Empty;

    [ValidTags] public List<string> Tags { get; set; } = new();

    [Required] public AccessType AccessType { get; set; } = AccessType.Personal;

    public string? Password { get; set; } = null;

    [Required] [ExpiryDate] public DateTime ExpiryDate { get; set; }
}