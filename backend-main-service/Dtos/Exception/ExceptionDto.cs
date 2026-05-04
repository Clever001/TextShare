using System.ComponentModel.DataAnnotations;

namespace DocShareApi.Dtos.Exception;

public sealed class ExceptionDto {
    [Required]
    public string Code { get; init; } = "";
    [Required]
    public string Description { get; init; } = "";
    [Required]
    public List<string>? Details { get; init; }
}
