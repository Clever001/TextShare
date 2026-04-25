using System.ComponentModel.DataAnnotations.Schema;

namespace DocShareApi.Models;

[Table("FriendPairs")]
public class FriendPair {
    public string FirstUserId { get; set; }
    public AppUser FirstUser { get; set; }
    public string SecondUserId { get; set; }
    public AppUser SecondUser { get; set; }
}