using System.ComponentModel.DataAnnotations;

namespace TextShareApi.Dtos.Accounts;

public class ProcessFriendRequestDto {
    [Required] public bool AcceptRequest { get; set; }
}