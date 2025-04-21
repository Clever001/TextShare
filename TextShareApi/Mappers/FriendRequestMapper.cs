using TextShareApi.Dtos.FriendRequest;
using TextShareApi.Models;

namespace TextShareApi.Mappers;

public static class FriendRequestMapper {
    public static FriendRequestDto ToDto(this FriendRequest fr) {
        return new FriendRequestDto() {SenderName = fr.Sender.UserName, RecipientName = fr.Recipient.UserName, IsAccepted = fr.IsAccepted};
    }
}