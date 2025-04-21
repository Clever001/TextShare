namespace TextShareApi.Dtos.Text;

public class PatchTextDto {
    public string? Content { get; set; }
    public bool Personal { get; set; }
    public bool ByReferencePublic { get; set; }
    public bool ByReferenceAuthorized { get; set; }
    public bool OnlyFriends { get; set; }
    public string? Password { get; set; }
}