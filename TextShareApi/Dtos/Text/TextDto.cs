namespace TextShareApi.Dtos.Text;

public class TextDto {
    public string Id { get; set; }
    public string Content { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public string OwnerName { get; set; }
}