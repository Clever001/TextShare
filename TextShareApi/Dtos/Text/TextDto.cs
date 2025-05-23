namespace TextShareApi.Dtos.Text;

public class TextDto {
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Syntax { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new List<string>();
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public string OwnerName { get; set; } = string.Empty;
    public string AccessType { get; set; } = string.Empty;
    public bool HasPassword { get; set; }
    public DateTime ExpiryDate { get; set; }
}