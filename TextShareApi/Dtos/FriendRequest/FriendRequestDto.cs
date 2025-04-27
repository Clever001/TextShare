namespace TextShareApi.Dtos.FriendRequest;

public class FriendRequestDto {
    public string SenderName { get; set; } = string.Empty;
    public string RecipientName { get; set; } = string.Empty;
    public bool? IsAccepted { get; set; }
}