using TextShareApi.Dtos.Text;
using TextShareApi.Models;

namespace TextShareApi.Mappers;

public static class TextMapper {
    /// <summary>
    /// Converts text model to dto. Using provided userName if such exists. 
    /// Remember to use .Include() Method if you don't provide username.
    /// </summary>
    public static TextDto ToTextDto(this Text text, string? userName = null) {
        return new() {
            Id = text.Id,
            Content = text.Content,
            CreatedOn = text.CreatedOn,
            UpdatedOn = text.UpdatedOn,
            OwnerName = userName ?? text.AppUser.UserName,
        };
    }
}