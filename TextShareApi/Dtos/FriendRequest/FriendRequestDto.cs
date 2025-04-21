namespace TextShareApi.Dtos.FriendRequest;

public class FriendRequestDto {
    public string SenderName { get; set; }
    public string RecipientName { get; set; }
    public bool? IsAccepted { get; set; }
}