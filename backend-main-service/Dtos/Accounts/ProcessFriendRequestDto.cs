using System.ComponentModel.DataAnnotations;

namespace DocShareApi.Dtos.Accounts;

public class ProcessFriendRequestDto {
    [Required] public bool AcceptRequest { get; set; }
}