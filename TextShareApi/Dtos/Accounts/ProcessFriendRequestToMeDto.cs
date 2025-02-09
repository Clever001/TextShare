using System.ComponentModel.DataAnnotations;

namespace TextShareApi.Dtos.Accounts;

public class ProcessFriendRequestToMeDto {
    [Required] 
    public string UserName { get; set; } = string.Empty;
    [Required]
    public bool AcceptRequest { get; set; }
}