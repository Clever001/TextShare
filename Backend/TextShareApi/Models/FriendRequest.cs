using System.ComponentModel.DataAnnotations.Schema;

namespace TextShareApi.Models;

[Table("FriendRequests")]
public class FriendRequest {
    public string SenderId { get; set; }
    public AppUser Sender { get; set; }
    public string RecipientId { get; set; }
    public AppUser Recipient { get; set; }
    public bool? IsAccepted { get; set; }
}