using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Identity;
using TextShareApi.Attributes;
using TextShareApi.Models.Enums;

namespace TextShareApi.Dtos.Text;

[OnlyOneTrue(nameof(Personal), nameof(ByReferencePublic), 
    nameof(ByReferenceAuthorized), nameof(OnlyFriends))]
public class CreateTextDto {
    public string? Text { get; set; }
    public bool Personal { get; set; }
    public bool ByReferencePublic { get; set; }
    public bool ByReferenceAuthorized { get; set; }
    public bool OnlyFriends { get; set; }
    public string? Password { get; set; }
}