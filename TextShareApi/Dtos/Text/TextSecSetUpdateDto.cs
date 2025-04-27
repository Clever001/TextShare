namespace TextShareApi.Dtos.Text;

public class TextSecSetUpdateDto {
    public string? AccessType { get; set; }
    public string? Password { get; set; }
    public bool UpdatePassword { get; set; }
}